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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Core.Utilities;
using LA = ASP.Application.UseCases.LocalAuthorities.DTO;
using MAT = ASP.Application.UseCases.MultiAcademyTrusts.DTO;
using ASP.Application.UseCases.BlobStorageDemoFileDownload;
using ASP.Application.UseCases.BlobStorageDemoZipFileDownload;

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

            return ApiGet<ContentTemplate>(url, queryString);
        }

        public Task<Result<List<ContentTemplate>>> GetAllContentTemplates()
        {
            var url = "/api/GetAllContentTemplates";

            return ApiGet<List<ContentTemplate>>(url, null);
        }

        public Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request)
        {
            var url = "/api/UpdateContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);

            request.Revision.IfSome(value =>
            {
                queryString = queryString.Add("revision", value);
            });

            return ApiPost<Done>(url, queryString, request.ContentTemplate);
        }

        public Task<Result<GetAvailableSchoolDownloadsResponse>> GetAvailableSchoolDownloads(GetAvailableSchoolDownloadsRequest request)
        {
            var url = "/api/GetAvailableSchoolDownloads";
            var queryString = QueryString.Create("urn", request.Urn);

            request.Year.IfSome(value =>
            {
               queryString = queryString.Add("year", value.ToString());
            });

            return ApiGet<GetAvailableSchoolDownloadsResponse>(url, queryString);
        }

        public Task<Result<FileStreamResponse>> DownloadAsZipFile(DownloadAsZipFileRequest request)
        {
            var url = "/api/DownloadAsZipFile";
            var queryString = QueryString.Create("fileType", request.FileType.ToString());

            foreach (var id in request.DownloadIds)
            {
                queryString = queryString.Add("downloadIds", id);
            }

            return ApiGetFileStream(url, queryString);
        }

        public Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request)
        {
            var url = "/api/GetEstablishmentDetails";
            var queryString = QueryString.Create("urn", request.Urn);

            return ApiGet<EstablishmentDetailsDTO>(url, queryString);
        }

        public Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(GetAllEstablishmentsRequest request)
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

            return ApiGet<ScopedResultsPage<EstablishmentListingDTO>>(url, queryString);
        }

        public Task<Result<SearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(EstablishmentSearchRequest request)
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

            return ApiGet<SearchResultsPage<EstablishmentListingDTO>>(url, queryString);
        }

        public Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request)
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

            return ApiGet<SearchSuggestionsResult<EstablishmentSuggestionDTO>>(url, queryString);
        }

        public Task<Result<LA.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request)
        {
            var url = "/api/GetLocalAuthority";
            var queryString = QueryString.Create("code", request.Code);

            return ApiGet<LA.LocalAuthorityDTO>(url, queryString);
        }

        public Task<Result<ResultsPage<LA.LocalAuthorityDTO>>> GetAllLocalAuthorities(GetAllLocalAuthoritiesRequest request)
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

            return ApiGet<ResultsPage<LA.LocalAuthorityDTO>>(url, queryString);
        }

        public Task<Result<MAT.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request)
        {
            var url = "/api/GetMultiAcademyTrust";
            var queryString = QueryString.Create("id", request.Id);

            return ApiGet<MAT.MultiAcademyTrustDTO>(url, queryString);
        }

        public Task<Result<FileStreamResponse>> BlobStorageDemoFileDownload(BlobStorageDemoFileDownloadRequest request)
        {
            var url = "/api/BlobStorageDemoFileDownload";
            var queryString = QueryString.Create("container", request.Container)
                .Add("filepath", request.Filepath);

            return ApiGetFileStream(url, queryString);
        }

        public Task<Result<FileStreamResponse>> BlobStorageDemoZipFileDownload(BlobStorageDemoZipFileDownloadRequest request)
        {
            var url = "/api/BlobStorageDemoZipFileDownload";
            var queryString = QueryString.Create("container", request.Container)
                .Add("filepath", request.Filepath);

            return ApiGetFileStream(url, queryString);
        }

        private async Task<Result<T>> ApiGet<T>(string url, QueryString? queryString)
            where T : notnull
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new HttpRequestMessage(HttpMethod.Get, $"https://localhost{url}{queryString}"));

                return await ToResult<T>(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<T>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private async Task<Result<T>> ApiPost<T>(string url, QueryString? queryString, object body)
            where T : notnull
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new HttpRequestMessage(HttpMethod.Post, $"https://localhost{url}{queryString}") {
                    Content = new StringContent(JsonHelper.Serialize(body))
                });

                return await ToResult<T>(response);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<T>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private async Task<Result<FileStreamResponse>> ApiGetFileStream(string url, QueryString? queryString)
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new HttpRequestMessage(HttpMethod.Get, $"https://localhost{url}{queryString}"));
                var filename = response.Content.Headers.ContentDisposition?.FileName ?? "";
                var stream = await response.Content.ReadAsStreamAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "";

                return new FileStreamResponse(filename, stream, contentType);
            }
            catch (Exception ex)
            {
                return Result.Unexpected<FileStreamResponse>($"{ApiConnectionError}{ex.Message}", ex.StackTrace);
            }
        }

        private async Task<Result<T>> ToResult<T>(HttpResponseMessage response)
            where T : notnull
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => JsonHelper.DeserializeNotNull<T>(content),

                    HttpStatusCode.NotFound => Result.NotFound<T>(
                        ApiError + StringHelper.RemoveFromStart(_notFoundPrefix, content)),

                    HttpStatusCode.BadRequest => Result.Invalid<T>(
                        ApiError + StringHelper.RemoveFromStart(_invalidPrefix, content)),

                    HttpStatusCode.Forbidden => Result.NotAllowed<T>(
                        ApiError + StringHelper.RemoveFromStart(_notAllowedPrefix, content)),

                    HttpStatusCode.MethodNotAllowed => Result.Invalid<T>(
                        ApiError + StringHelper.RemoveFromStart(_methodNotAllowedPrefix, content)),

                    _ => JsonHelper.DeserializeNotNull<UnexpectedError>(content)
                        .Then(e => Result.Unexpected<T>(
                            ApiError + StringHelper.RemoveFromStart(_unexpectedPrefix, e.Message),
                            _options.ShowStackTrace ? e.StackTrace : null
                        ))
                };
            }
            catch (Exception ex)
            {
                return Result.Unexpected<T>($"{ApiResponseError}{ex.Message}", ex.StackTrace);
            }
        }
    }
}