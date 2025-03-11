using ASP.Api.Client.Schools;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.UseCases.GetAllSchools;
using ASP.Domain.Schools.UseCases.SchoolSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Schools;

public class SchoolsGetAll : ApiFunction
{
    private readonly ILogger<SchoolsGetAll> _logger;
    private readonly IGetAllSchoolsUseCase _getAll;
    private readonly ISchoolSearchUseCase _search;
    private readonly ApiResultConverter _resultConverter;

    public SchoolsGetAll(
        ILogger<SchoolsGetAll> logger,
        IGetAllSchoolsUseCase getAll,
        ISchoolSearchUseCase search,
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

    [OpenApiOperation(operationId: "SchoolsGetAll", tags: ["Schools"], 
        Description = "Retrieves a list of schools within a specified scope.")]
    [OpenApiParameter(name: "searchTerm", In = ParameterLocation.Query, Required = false, 
        Description = "A search term to filter schools by.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = false, 
        Description = "Scope of the search, e.g.: `LA`, `MAT`, `Diocese`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = false, 
        Description = "An identifier for the selected scope, e.g.: LA code, MAT UID or Diocese name (case insensitive).")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, 
        Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, 
        Description = "The number of results to return per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ResultsPage<SchoolListing>), 
        Description = "A list of schools within the specified scope matching the given search term.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: Could not find any schools within the given scope matching the given search term.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method POST/PUT/DELETE is not allowed.")]
    
    [Function("SchoolsGetAll")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "post", "delete", Route = "schools")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from searchTerm in request.ValidateQueryStringParameter("searchTerm", p => p.IsOptional().IsNotEmpty())
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsOptional().IsNotEmpty().IsEnum<SchoolAccessScopeType>())
            from scopeId in request.ValidateQueryStringParameter("scopeId", p => p.IsRequiredIf(scope.HasValue).IsNotEmpty())
            from page in request.ValidateQueryStringParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateQueryStringParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in searchTerm.Match(
                value => _search.HandleRequest(new SchoolSearchRequest(
                    value,
                    SchoolAccessScopeInfo.Create(scope, scopeId),
                    page,
                    resultsPerPage)),
                () => _getAll.HandleRequest(new GetAllSchoolsRequest(
                    SchoolAccessScopeInfo.Create(scope, scopeId),
                    page,
                    resultsPerPage)))
            select response.MapResultsPage(p => p.ForApiClientAsListing());

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}