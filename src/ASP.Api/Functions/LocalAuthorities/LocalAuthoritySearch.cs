using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;
using ASP.Core.Pagination;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.LocalAuthorities;

public class LocalAuthoritySearch : ApiFunction
{
    private readonly ILogger<LocalAuthoritySearch> _logger;
    private readonly ILocalAuthoritySearch _useCase;
    private readonly ApiResultConverter _resultConverter;

    public LocalAuthoritySearch(
        ILogger<LocalAuthoritySearch> logger,
        ILocalAuthoritySearch useCase,
        ApiResultConverter resultConverter
    )
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
            ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("LocalAuthoritySearch")]
    [OpenApiOperation(operationId: "LocalAuthoritySearch", tags: ["Local Authorities"], Description = "Allows searching for local authorities by a search term.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Summary = "Search term for local authority names", Description = "The term used to search local authorities.")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "Page number for paginated results", Description = "Page number for retrieving paginated search results.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Type = typeof(int), Summary = "Number of results per page", Description = "Number of results to retrieve per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SearchResultsPage<Client.LookupValueWithCode>), Description = "The list of matching local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Invalid input parameters or query format.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No matching Local Authorities for the search term.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from searchTerm in request.ValidateParameter("searchTerm", p => p.IsRequired())
            from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new LocalAuthoritySearchRequest(
                searchTerm,
                page,
                resultsPerPage
            ))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}