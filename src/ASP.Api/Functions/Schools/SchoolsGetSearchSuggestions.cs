using ASP.Api.Client.Schools;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.UseCases.SchoolSearchSuggestions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Schools;

public class SchoolsGetSearchSuggestions : ApiFunction
{
    private readonly ILogger<SchoolsGetSearchSuggestions> _logger;
    private readonly ISchoolSearchSuggestionsUseCase _useCase;
    private readonly ApiResultConverter _resultConverter;

    public SchoolsGetSearchSuggestions(
        ILogger<SchoolsGetSearchSuggestions> logger,
        ISchoolSearchSuggestionsUseCase useCase,
        ApiResultConverter resultConverter)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
            ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("SchoolsGetSearchSuggestions")]
    [OpenApiOperation(operationId: "SchoolsGetSearchSuggestions", tags: ["Schools"], Description = "Retrieves a list of schools based on a search term within a specified scope.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = true, Description = "The search term for search suggestions.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = false, Description = "Scope of the search, e.g., `LA`, `MAT` or `Diocese`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, LA code, MAT UID or Diocese name")]
    [OpenApiParameter(name: "maxSuggestions", In = ParameterLocation.Query, Required = false, Description = "The maximum number of search suggestions to return.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<SchoolSuggestion>), Description = "A list of school search suggestions.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No matching suggestions for the search term and scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "schools/search-suggestions")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from searchTerm in request.ValidateQueryStringParameter("searchTerm", p => p.IsRequired().IsNotEmpty())
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsOptional().IsNotEmpty().IsEnum<SchoolAccessScopeType>())
            from scopeId in request.ValidateQueryStringParameter("scopeId", p => p.IsRequiredIf(scope.HasValue).IsNotEmpty())
            from maxSuggestions in request.ValidateQueryStringParameter("maxSuggestions", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new SchoolSearchSuggestionsRequest(
                searchTerm,
                SchoolAccessScopeInfo.Create(scope, scopeId),
                maxSuggestions
            ))
            select response.MapList(l => l.ForApiClientAsSuggestion());
        ;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}