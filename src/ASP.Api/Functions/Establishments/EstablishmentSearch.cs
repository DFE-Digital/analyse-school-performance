using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.UseCases.EstablishmentSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Establishments;

public class EstablishmentSearch : ApiFunction
{
    private readonly ILogger<EstablishmentSearch> _logger;
    private readonly IEstablishmentSearch _useCase;
    private readonly ApiResultConverter _resultConverter;

    public EstablishmentSearch(
        ILogger<EstablishmentSearch> logger,
        IEstablishmentSearch useCase,
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

    [Function("EstablishmentSearch")]
    [OpenApiOperation(operationId: "EstablishmentSearch", tags: ["Establishments"], Description = "Retrieves a list of establishments based on a search term within a specified scope.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Description = "The search term for establishments.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = true, Description = "Scope of the search, e.g, `All`, `LA`, `MAT`, `Diocese`.")]
    [OpenApiParameter(name: "scopeIdentifier", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, `LA Code`, `MAT UID`, `Diocese name`")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Description = "The number of results per page for pagination (optional).")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ScopedSearchResultsPage<Client.Establishments.EstablishmentListing>), Description = "A paginated list of establishments matching the search criteria.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No matching establishments for the search term and scope.")]
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
            from scope in request.ValidateParameter("scope", p => p.IsRequired().IsEnum<EstablishmentScopeType>())
            from scopeIdentifier in request.ValidateParameter("scopeIdentifier", p => p.IsRequiredIf(scope != EstablishmentScopeType.All))
            from page in request.ValidateParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new EstablishmentSearchRequest(
                searchTerm,
                scope,
                scopeIdentifier,
                page,
                resultsPerPage
            ))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}