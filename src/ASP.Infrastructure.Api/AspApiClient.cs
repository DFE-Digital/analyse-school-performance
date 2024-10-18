using ASP.Api;
using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Core.Utilities;

namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Client used for accessing the ASP API. The API endpoints map one-to-one with application UseCases, so each 
    /// endpoint is represented by a function with the UseCase name, which takes the UseCase request as an input
    /// and returns the UseCase response. This client is designed to communicate with the API via a transport layer
    /// abstraction, which could represent a HTTP connection (<see cref="HttpTransportLayer"/>), or an in-process 
    /// connection to the API function objects in memory (<see cref="InProcessTransportLayer"/>).
    /// The request and response bodies of the transport layer are text strings, to ensure serialization is
    /// handled on this side of the boundary between the client and the API, so that regardless of which transport layer 
    /// implementation is used, the serialization code is exercised. This ensures any issues with 
    /// </summary>
    public class AspApiClient : IAspApiClient
    {
        private const string ApiError = "API error: ";
        private const string ApiConnectionError = "Error connecting to the API: ";
        private const string ApiResponseError = "Error reading the response from the API: ";

        private static readonly string _unexpectedPrefix = Error.Unexpected("", null).MessagePrefix;
        private static readonly string _notFoundPrefix = Error.NotFound("").MessagePrefix;
        private static readonly string _invalidPrefix = Error.Invalid("").MessagePrefix;
        private static readonly string _notAllowedPrefix = Error.NotAllowed("").MessagePrefix;
        private static readonly string _methodNotAllowedPrefix = new MethodNotAllowedError("", []).MessagePrefix;

        private readonly ITransportLayer _transportLayer;
        private readonly ErrorHandlingOptions _options;
        private readonly ILogger<AspApiClient> _logger;

        public AspApiClient(ITransportLayer transportLayer, IOptions<ErrorHandlingOptions> options, ILogger<AspApiClient> logger)
        {
            _transportLayer = transportLayer ?? throw new ArgumentNullException(nameof(transportLayer));
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request)
        {
            var url = "/api/ViewContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);

            request.Revision.IfSome(value =>
            {
                queryString = queryString.Add("revision", value);
            });

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<ContentTemplate>(response)
                select result;
        }

        public Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request)
        {
            var url = "/api/UpdateContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);

            request.Revision.IfSome(value =>
            {
                queryString = queryString.Add("revision", value);
            });

            return 
                from _ in ApiPost(url, queryString, request.ContentTemplate)
                select Result.Done;
        }

        public Task<Result<List<ContentTemplate>>> GetAllContentTemplates()
        {
            var url = "/api/GetAllContentTemplates";

            return 
                from response in ApiGet(url, null)
                from result in JsonHelper.DeserializeNotNull<List<ContentTemplate>>(response)
                select result;
        }

        public async Task<Result<GetAvailableSchoolDownloadsResponse>> GetAvailableSchoolDownloads(GetAvailableSchoolDownloadsRequest request)
        {
            var url = "/api/GetAvailableSchoolDownloads";
            var queryString = QueryString.Create("urn", request.Urn);

            request.Year.IfSome(value =>
            {
               queryString = queryString.Add("year", value.ToString());
            });

            var data = await ApiGet(url, queryString)
                .Then(response => JsonHelper.DeserializeNotNull<GetAvailableSchoolDownloadsResponse>(response));

            return data;
        }

        public async Task<Result<ActionResult>> DownloadAsZipFile(DownloadAsZipFileRequest request)
        {
            var url = "/api/DownloadAsZipFile";
            var queryString = QueryString.Create("fileType", request.FileType.ToString());

            foreach (var id in request.DownloadIds)
            {
                queryString = queryString.Add("downloadIds", id);
            }

            return await ApiGetZip(url, queryString);
        }

        public Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request)
        {
            var url = "/api/GetEstablishmentDetails";
            var queryString = QueryString.Create("urn", request.Urn);

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<EstablishmentDetailsDTO>(response)
                select result;
        }

        public async Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(
        GetAllEstablishmentsRequest request)
        {
            var url = "/api/GetAllEstablishments";
            var queryString = QueryString.Create("scope", request.ScopeType.ToString());

            request.ScopeIdentifier.IfSome(value =>
            {
                queryString = queryString.Add("scopeIdentifier", value);
            });

            request.Page.IfSome(value =>
            {
                queryString = queryString.Add("page", value.ToString());
            });

            request.ResultsPerPage.IfSome(value =>
            {
                queryString = queryString.Add("resultsPerPage", value.ToString());
            });

            return await ApiGet(url, queryString)
                .Then(response =>
                    JsonHelper.DeserializeNotNull<ScopedResultsPage<EstablishmentListingDTO>>(response));
        }

        public Task<Result<SearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(
            EstablishmentSearchRequest request)
        {
            var url = "/api/EstablishmentSearch";
            var queryString = QueryString.Create("searchTerm", request.SearchTerm);

            queryString = queryString.Add("scope", request.ScopeType.ToString());


            request.ScopeIdentifier.IfSome(value =>
            {
                queryString = queryString.Add("scopeIdentifier", value);
            });

            request.Page.IfSome(value =>
            {
                queryString = queryString.Add("page", value.ToString());
            });

            request.ResultsPerPage.IfSome(value =>
            {
                queryString = queryString.Add("resultsPerPage", value.ToString());
            });

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<SearchResultsPage<EstablishmentListingDTO>>(response)
                select result;
        }

        public Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>>
            EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request)
        {
            var url = "/api/EstablishmentSearchSuggestions";
            var queryString = QueryString.Create("searchTerm", request.SearchTerm);

            queryString = queryString.Add("scope", request.ScopeType.ToString());

            request.ScopeIdentifier.IfSome(value =>
            {
                queryString = queryString.Add("scopeIdentifier", value);
            });

            request.MaxSuggestions.IfSome(value =>
            {
                queryString = queryString.Add("maxSuggestions", value.ToString());
            });

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<SearchSuggestionsResult<EstablishmentSuggestionDTO>>(response)
                select result;
        }

        public Task<Result<Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request)
        {
            var url = "/api/GetLocalAuthority";
            var queryString = QueryString.Create("code", request.Code);

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>(response)
                select result;
        }

        public Task<Result<Application.UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request)
        {
            var url = "/api/GetMultiAcademyTrust";
            var queryString = QueryString.Create("id", request.Id);

            return 
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<Application.UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO>(response)
                select result;
        }
        
        public Task<Result<ResultsPage<ASP.Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>>> GetAllLocalAuthorities(
            GetAllLocalAuthoritiesRequest request)
        {
            var url = "/api/GetAllLocalAuthorities";
            var queryString = new QueryString();
            
            request.Page.IfSome(value =>
            {
                queryString = queryString.Add("page", value.ToString());
            });

            request.ResultsPerPage.IfSome(value =>
            {
                queryString = queryString.Add("resultsPerPage", value.ToString());
            });

            return
                from response in ApiGet(url, queryString)
                from result in JsonHelper.DeserializeNotNull<ResultsPage<ASP.Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>>(response)
                select result;
        }

        private async Task<Result<string>> ApiGet(string url, QueryString? queryString)
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new TransportLayerRequest
                {
                    Method = HttpMethods.Get,
                    Path = url,
                    QueryString = queryString.ToString()
                });

                return ToResult(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<string>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private async Task<Result<string>> ApiPost(string url, QueryString? queryString, object body)
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new TransportLayerRequest
                {
                    Method = HttpMethods.Post,
                    Path = url,
                    QueryString = queryString.ToString(),
                    Body = JsonHelper.Serialize(body)
                });

                return ToResult(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<string>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private async Task<Result<ActionResult>> ApiGetZip(string url, QueryString? queryString)
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new TransportLayerRequest {
                    Method = HttpMethods.Get,
                    Path = url,
                    QueryString = queryString.ToString()
                });

                var contentDispositionHeader = response.Headers["Content-Disposition"];
                var fileName = contentDispositionHeader
                    .Split(';')
                    .Select(part => part.Trim())
                    .FirstOrDefault(part => part.StartsWith("filename="))?
                    .Split('=')[1]
                    .Trim('"');

                return new FileStreamResult(response.BodyStream!, "application/octet-stream") {
                    FileDownloadName = fileName
                };
            }
            catch (Exception ex)
            {
                return Result.Unexpected<ActionResult>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private Result<string> ToResult(TransportLayerResponse response)
        {
            Result<string> result;

            try
            {
                var content = response.BodyString ?? "";

                Result<string> handleUnexpected(string content)
                {
                    return 
                        from e in JsonHelper.DeserializeNotNull<UnexpectedError>(content)
                        from result in Result.Unexpected<string>(
                            ApiError + StringHelper.RemoveFromStart(_unexpectedPrefix, e.Message),
                            _options.ShowStackTrace ? e.StackTrace : null
                        )
                        select result;
                }

                result = response.StatusCode switch
                {
                    (int)HttpStatusCode.OK => Result.Success(content ?? ""),

                    (int)HttpStatusCode.NotFound => Result.NotFound<string>(
                        ApiError + StringHelper.RemoveFromStart(_notFoundPrefix, content)),

                    (int)HttpStatusCode.BadRequest => Result.Invalid<string>(
                        ApiError + StringHelper.RemoveFromStart(_invalidPrefix, content)),

                    (int)HttpStatusCode.Forbidden => Result.NotAllowed<string>(
                        ApiError + StringHelper.RemoveFromStart(_notAllowedPrefix, content)),

                    (int)HttpStatusCode.MethodNotAllowed => Result.Invalid<string>(
                        ApiError + StringHelper.RemoveFromStart(_methodNotAllowedPrefix, content)),

                    _ => handleUnexpected(content)
                };
            }
            catch (Exception ex)
            {
                result = Result.Unexpected<string>($"{ApiResponseError}{ex.Message}", ex.StackTrace);
            }

            return result;
        }
    }
}