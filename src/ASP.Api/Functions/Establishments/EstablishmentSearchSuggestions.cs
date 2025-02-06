using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Establishments;

public class EstablishmentSearchSuggestions : ApiFunction
{
    private readonly ILogger<EstablishmentSearchSuggestions> _logger;
    private readonly IEstablishmentSearchSuggestions _useCase;
    private readonly ApiResultConverter _resultConverter;

    public EstablishmentSearchSuggestions(
        ILogger<EstablishmentSearchSuggestions> logger,
        IEstablishmentSearchSuggestions useCase,
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

    [Function("EstablishmentSearchSuggestions")]
    [OpenApiOperation(operationId: "EstablishmentSearchSuggestions", tags: ["Establishments"], Description = "Retrieves a list of establishments based on a search term within a specified scope.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Description = "The search term for establishment suggestions.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = true, Description = "Scope of the search, e.g., `All`, `LA`, `MAT`, `Diocese`.")]
    [OpenApiParameter(name: "scopeIdentifier", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, `LA Code`, `MAT UID`, `Diocese name`")]
    [OpenApiParameter(name: "maxSuggestions", In = ParameterLocation.Query, Required = false, Description = "The maximum number of suggestions to return.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ScopedSearchSuggestionsList<Client.Establishments.EstablishmentSuggestion>), Description = "A list of establishment search suggestions.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No matching suggestions for the search term and scope.")]
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
            from maxSuggestions in request.ValidateParameter("maxSuggestions", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new EstablishmentSearchSuggestionsRequest(
                searchTerm,
                scope,
                scopeIdentifier,
                maxSuggestions
            ))
            select response.ForApiClient();
        ;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}