using ASP.Api;
using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;

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

        public async Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request)
        {
            var url = "/api/ViewContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);
            if (request.Revision != null)
            {
                queryString = queryString.Add("revision", request.Revision);
            }
            
             return await ApiGet(url, queryString)
                .Then(response => JsonHelper.DeserializeNotNull<ContentTemplate>(response));
        }

        public async Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request)
        {
            var url = "/api/UpdateContentTemplate";
            var queryString = QueryString.Create("id", request.ContentTemplateId);
            if (request.Revision != null)
            {
                queryString = queryString.Add("revision", request.Revision);
            }

            return await ApiPost(url, queryString, request.ContentTemplate)
                .Then<string, Done>(_ => Result.Done);
        }

        public async Task<Result<List<ContentTemplate>>> GetAllContentTemplates()
        {
            var url = "/api/GetAllContentTemplates";

            return await ApiGet(url, null)
                .Then(response => JsonHelper.DeserializeNotNull<List<ContentTemplate>>(response));
        }

        public async Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request)
        {
            var url = "/api/GetEstablishmentDetails";
            var queryString = QueryString.Create("urn", request.Urn);

            return await ApiGet(url, queryString)
                .Then(response => JsonHelper.DeserializeNotNull<EstablishmentDetailsDTO>(response));
        }

        public async Task<Result<SearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(
            EstablishmentSearchRequest request)
        {
            var url = "/api/EstablishmentSearch";
            var queryString = QueryString.Create("searchTerm", request.SearchTerm);

            queryString = queryString.Add("scope", request.ScopeType.ToString())
                .Add("scopeIdentifier", request.ScopeIdentifier);

            if (request.Page != null)
            {
                queryString = queryString.Add("page", request.Page.ToString() ?? "");
            }

            if (request.ResultsPerPage != null)
            {
                queryString = queryString.Add("resultsPerPage", request.ResultsPerPage.ToString() ?? "");
            }

            return await ApiGet(url, queryString)
                .Then(response =>
                    JsonHelper.DeserializeNotNull<SearchResultsPage<EstablishmentListingDTO>>(response));
        }

        public async Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>>
            EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request)
        {
            var url = "/api/EstablishmentSearchSuggestions";
            var queryString = QueryString.Create("searchTerm", request.SearchTerm);

            queryString = queryString.Add("scope", request.ScopeType.ToString())
                .Add("scopeIdentifier", request.ScopeIdentifier);

            if (request.MaxSuggestions != null)
            {
                queryString = queryString.Add("maxSuggestions", request.MaxSuggestions.ToString() ?? "");
            }

            return await ApiGet(url, queryString)
                .Then(response =>
                    JsonHelper.DeserializeNotNull<SearchSuggestionsResult<EstablishmentSuggestionDTO>>(
                        response));
        }
        
        public async Task<Result<Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request)
        {
            var url = "/api/GetLocalAuthority";
            var queryString = QueryString.Create("code", request.Code);
            return await ApiGet(url, queryString)
                .Then(response => JsonHelper.DeserializeNotNull<Application.UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>(response));
        }
        
        public async  Task<Result<Application.UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request)
        {
            var url = "/api/GetMultiAcademyTrust";
            var queryString = QueryString.Create("id", request.Id);
            return await ApiGet(url, queryString)
                .Then(response => JsonHelper.DeserializeNotNull<Application.UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO>(response));
        }

        private async Task<Result<string>> ApiGet(string url, QueryString? queryString)
        {
            try
            {
                var response = await _transportLayer.ExecuteRequest(new TransportLayerRequest { 
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

        private Result<string> ToResult(TransportLayerResponse response)
        {
            Result<string> result;

            try
            {
                var content = response.Body ?? "";

                Result<string> handleUnexpected(string content)
                {
                    return JsonHelper.DeserializeNotNull<UnexpectedError>(content)
                        .Then(e => Result.Unexpected<string>(
                            ApiError + StringHelper.RemoveFromStart(_unexpectedPrefix, e.Message), 
                            _options.ShowStackTrace ? e.StackTrace : null
                        ));
                }

                result = response.StatusCode switch {
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