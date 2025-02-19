using ASP.Api.Client.ContentTemplates;
using ASP.Api.Client.Downloads;
using ASP.Api.Client.Schools;
using ASP.Core.Network;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Core.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using LA = ASP.Api.Client.LocalAuthorities;
using MAT = ASP.Api.Client.MultiAcademyTrusts;

namespace ASP.Api.Client;

/// <summary>
/// Client used for accessing the ASP API. The API endpoints map one-to-one with application UseCases, so each 
/// endpoint is represented by a function with the UseCase name, which takes the UseCase request as an input
/// and returns the UseCase response. This client is designed to communicate with the API via a transport layer
/// abstraction, which could represent a HTTP connection (<see cref="HttpTransportLayer"/>), or an in-process 
/// connection to the API function objects in memory (e.g. <c>ASP.Infrastructure.InProcessApi.InProcessTransportLayer</c>).
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

    public Task<Result<List<ContentTemplate>>> ContentTemplatesGetAll(ContentTemplatesGetAllRequest request)
    {
        var url = "/api/content-templates";

        return ApiGet<List<ContentTemplate>>(url, null);
    }

    public Task<Result<ContentTemplate>> ContentTemplatesGetSingle(ContentTemplatesGetSingleRequest request)
    {
        var url = $"/api/content-templates/{Uri.EscapeDataString(request.ContentTemplateId)}";
        var queryString = QueryString.Empty;

        if (request.Revision != null)
        {
            queryString = queryString.Add("revision", request.Revision);
        };

        return ApiGet<ContentTemplate>(url, queryString);
    }

    public Task<Result<Done>> ContentTemplatesUpdateSingle(ContentTemplatesUpdateSingleRequest request)
    {
        var url = $"/api/content-templates/{Uri.EscapeDataString(request.ContentTemplateId)}";
        var queryString = QueryString.Empty;

        if (request.Revision != null)
        {
            queryString = queryString.Add("revision", request.Revision);
        }

        return ApiPost<Done>(url, queryString, request.ContentTemplate);
    }

    public Task<Result<DownloadsGetAllResponse>> DownloadsGetAll(DownloadsGetAllRequest request)
    {
        var url = "/api/downloads";
        var queryString = QueryString
            .Create("scope", request.ScopeType.ToString())
            .Add("scopeId", request.ScopeIdentifier);

        if (request.Year != null)
        {
            queryString = queryString.Add("year", request.Year.ToString());
        }

        return ApiGet<DownloadsGetAllResponse>(url, queryString);
    }

    public Task<Result<FileStreamResponse>> DownloadsGetPackage(DownloadsGetPackageRequest request)
    {
        var url = "/api/downloads/package";
        var queryString = QueryString
            .Create("scope", request.ScopeType.ToString())
            .Add("scopeIdentifier", request.ScopeIdentifier.ToString())
            .Add("fileType", request.FileType.ToString());

        foreach (var id in request.DownloadIds)
        {
            queryString = queryString.Add("downloadIds", id);
        }

        return ApiGetFileStream(url, queryString);
    }

    public Task<Result<SchoolDetails>> SchoolsGetSingle(SchoolsGetSingleRequest request)
    {
        var url = $"/api/schools/{Uri.EscapeDataString(request.Urn)}";

        return ApiGet<SchoolDetails>(url, null);
    }

    public Task<Result<ResultsPage<SchoolListing>>> SchoolsGetAll(SchoolsGetAllRequest request)
    {
        var url = "/api/schools";
        var queryString = QueryString.Empty;

        if (request.SearchTerm != null)
        {
            queryString = queryString.Add("searchTerm", request.SearchTerm);
        }

        if (request.Scope != null)
        {
            queryString = queryString.Add("scope", request.Scope.ScopeType.ToString());
            queryString = queryString.Add("scopeId", request.Scope.ScopeId);
        }

        if (request.Page != null)
        {
            queryString = queryString.Add("page", request.Page.ToString());
        }

        if (request.ResultsPerPage != null)
        {
            queryString = queryString.Add("resultsPerPage", request.ResultsPerPage.ToString());
        }

        return ApiGet<ResultsPage<SchoolListing>>(url, queryString);
    }

    public Task<Result<List<SchoolSuggestion>>> SchoolsGetSearchSuggestions(SchoolsGetSearchSuggestionsRequest request)
    {
        var url = $"/api/schools/search-suggestions";
        var queryString = QueryString.Create("searchTerm", request.SearchTerm);

        if (request.Scope != null)
        {
            queryString = queryString.Add("scope", request.Scope.ScopeType.ToString());
            queryString = queryString.Add("scopeId", request.Scope.ScopeId);
        }

        if (request.MaxSuggestions != null)
        {
            queryString = queryString.Add("maxSuggestions", request.MaxSuggestions.ToString());
        }

        return ApiGet<List<SchoolSuggestion>>(url, queryString);
    }

    public Task<Result<SchoolsGetAccessResponse>> SchoolsGetAccess(SchoolsGetAccessRequest request)
    {
        var url = $"/api/schools/{Uri.EscapeDataString(request.Urn)}/access";
        var queryString = QueryString.Empty;

        if (request.Scope != null)
        {
            queryString = queryString.Add("scope", request.Scope.ScopeType.ToString());
            queryString = queryString.Add("scopeId", request.Scope.ScopeId);
        }

        return ApiGet<SchoolsGetAccessResponse>(url, queryString);
    }

    public Task<Result<SchoolsGetLinkedSchoolsResponse>> SchoolsGetLinkedSchools(SchoolsGetLinkedSchoolsRequest request)
    {
        var url = $"/api/schools/{Uri.EscapeDataString(request.Urn)}/linked-schools";
        var queryString = QueryString.Create("urn", request.Urn);

        return ApiGet<SchoolsGetLinkedSchoolsResponse>(url, queryString);
    }

    public Task<Result<LookupValueWithCode>> LocalAuthoritiesGetSingle(LA.LocalAuthoritiesGetSingleRequest request)
    {
        var url = $"/api/local-authorities/{Uri.EscapeDataString(request.Code)}";
        var queryString = QueryString.Empty;

        return ApiGet<LookupValueWithCode>(url, queryString);
    }

    public Task<Result<ResultsPage<LookupValueWithCode>>> LocalAuthoritiesGetAll(LA.LocalAuthoritiesGetAllRequest request)
    {
        var url = "/api/local-authorities";
        var queryString = QueryString.Empty;

        if (request.SearchTerm != null)
        {
            queryString = queryString.Add("searchTerm", request.SearchTerm);
        }

        if (request.Page != null)
        {
            queryString = queryString.Add("page", request.Page.ToString());
        }

        if (request.ResultsPerPage != null)
        {
            queryString = queryString.Add("resultsPerPage", request.ResultsPerPage.ToString());
        }

        return ApiGet<ResultsPage<LookupValueWithCode>>(url, queryString);
    }

    public Task<Result<List<LookupValueWithCode>>>
        LocalAuthoritiesGetSearchSuggestions(LA.LocalAuthoritiesGetSearchSuggestionsRequest request)
    {
        var url = "/api/local-authorities/search-suggestions";
        var queryString = QueryString.Create("searchTerm", request.SearchTerm);

        if (request.MaxSuggestions != null)
        {
            queryString = queryString.Add("maxSuggestions", request.MaxSuggestions.ToString());
        }

        return ApiGet<List<LookupValueWithCode>>(url, queryString);
    }

    public Task<Result<LookupValueWithId>> MultiAcademyTrustsGetSingle(MAT.MultiAcademyTrustsGetSingleRequest request)
    {
        var url = $"/api/multi-academy-trusts/{Uri.EscapeDataString(request.Uid)}";
        var queryString = QueryString.Empty;

        return ApiGet<LookupValueWithId>(url, queryString);
    }

    private async Task<Result<T>> ApiGet<T>(string url, QueryString? queryString)
        where T : notnull
    {
        try
        {
            var response = await _transportLayer.ExecuteRequest(new HttpRequestMessage(HttpMethod.Get, $"https://localhost{url}{queryString}"));

            return await ToResult<T>(url, response);
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

            return await ToResult<T>(url, response);
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

            if (response.IsSuccessStatusCode is false)
            {
                return await ToResult<FileStreamResponse>(url, response);
            }

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

    private async Task<Result<T>> ToResult<T>(string url, HttpResponseMessage response)
        where T : notnull
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            var prefix = $"{ApiError} {url} ";

            return response.StatusCode switch {
                HttpStatusCode.OK => JsonHelper.DeserializeNotNull<T>(content),

                HttpStatusCode.NotFound => Result.NotFound<T>(
                    prefix + StringHelper.RemoveFromStart(_notFoundPrefix, content)),

                HttpStatusCode.BadRequest => Result.Invalid<T>(
                    prefix + StringHelper.RemoveFromStart(_invalidPrefix, content)),

                HttpStatusCode.Forbidden => Result.NotAllowed<T>(
                    prefix + StringHelper.RemoveFromStart(_notAllowedPrefix, content)),

                HttpStatusCode.MethodNotAllowed => Result.Invalid<T>(
                    prefix + StringHelper.RemoveFromStart(_methodNotAllowedPrefix, content)),

                _ => JsonHelper.DeserializeNotNull<UnexpectedError>(content)
                    .Then(e => Result.Unexpected<T>(
                        prefix + StringHelper.RemoveFromStart(_unexpectedPrefix, e.Message),
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