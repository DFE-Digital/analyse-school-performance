using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.UseCases.GetAllEstablishments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Establishments;

public class GetAllEstablishments : ApiFunction
{
    private readonly ILogger<GetAllEstablishments> _logger;
    private readonly IGetAllEstablishments _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAllEstablishments(
        ILogger<GetAllEstablishments> logger,
        IGetAllEstablishments useCase,
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

    [Function("GetAllEstablishments")]
    [OpenApiOperation(operationId: "GetAllEstablishments", tags: ["Establishments"], Description = "Retrieves a list of establishments within a specified scope.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = false, Description = "Scope of the search, e.g., `LA`, `MAT`, `Diocese`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, LA code, MAT UID or Diocese name")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Description = "The number of results to return per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ScopedResultsPage<Client.Establishments.EstablishmentListing>), Description = "A list of establishments within the specified scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any establishments within the given scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "schools")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsOptional().IsEnum<EstablishmentScopeType>())
            from scopeId in request.ValidateQueryStringParameter("scopeId", p => p.IsRequiredIf(scope.HasValue))
            from page in request.ValidateQueryStringParameter("page", p => p.IsOptional().IsNumeric())
            from resultsPerPage in request.ValidateQueryStringParameter("resultsPerPage", p => p.IsOptional().IsNumeric())
            from response in _useCase.HandleRequest(new GetAllEstablishmentsRequest(
                EstablishmentScopeInfo.Create(scope, scopeId),
                page,
                resultsPerPage
            ))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}