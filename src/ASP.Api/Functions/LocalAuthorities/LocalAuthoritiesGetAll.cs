using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.LocalAuthorities;

public class LocalAuthoritiesGetAll : ApiFunction
{
    private readonly ILogger<LocalAuthoritiesGetAll> _logger;
    private readonly IGetAllLocalAuthorities _getAll;
    private readonly ILocalAuthoritySearch _search;
    private readonly ApiResultConverter _resultConverter;

    public LocalAuthoritiesGetAll(
        ILogger<LocalAuthoritiesGetAll> logger,
        IGetAllLocalAuthorities getAll,
        ILocalAuthoritySearch search,
        ApiResultConverter resultConverter
    )
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _getAll = getAll
            ?? throw new ArgumentNullException(nameof(getAll));

        _search = search
            ?? throw new ArgumentNullException(nameof(search));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("LocalAuthoritiesGetAll")]
    [OpenApiOperation(operationId: "LocalAuthoritiesGetAll", tags: ["Local Authorities"], Description = "Retrieves a paginated list of all local authorities.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = false, Summary = "Search term for local authority names", Description = "The term used to search local authorities.")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Description = "The number of results to return per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResultsPage<Client.LookupValueWithCode>), Description = "A paginated list of local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "local-authorities")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from searchTerm in request.ValidateQueryStringParameter("searchTerm", p => p.IsOptional().IsNotEmpty())
            from page in request.ValidateQueryStringParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateQueryStringParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in searchTerm.Match(
                value => _search.HandleRequest(new LocalAuthoritySearchRequest(
                    value,
                    page,
                    resultsPerPage)),
                () => _getAll.HandleRequest(new GetAllLocalAuthoritiesRequest(
                    page,
                    resultsPerPage)))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}