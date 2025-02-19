using ASP.Core.Results;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Establishments;

public class IsEstablishmentAccessibleInScope : ApiFunction
{
    private readonly ILogger<GetAllEstablishments> _logger;
    private readonly IIsEstablishmentAccessibleInScope _useCase;
    private readonly ApiResultConverter _resultConverter;

    public IsEstablishmentAccessibleInScope(
        ILogger<GetAllEstablishments> logger,
        IIsEstablishmentAccessibleInScope useCase,
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

    [Function("IsEstablishmentAccessibleInScope")]
    [OpenApiOperation(operationId: "IsEstablishmentAccessibleInScope", tags: ["Establishments"], Description = "Determines whether an establishment is accessible within a specified scope.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Path, Required = true, Description = "The URN of the establishment.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = false, Description = "Scope to check to determine the establishment's accessibilty, e.g., `LA`, `MAT` or `Diocese`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, LA code, MAT UID or Diocese name")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Client.Establishments.IsEstablishmentAccessibleInScopeResponse), Description = "A response object indicating whether the establishment is accessible within the given scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any establishments within the given scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "schools/{urn:int}/access")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidateRouteParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsOptional().IsEnum<EstablishmentScopeType>())
            from scopeId in request.ValidateQueryStringParameter("scopeId", p => p.IsRequiredIf(scope.HasValue))
            from response in _useCase.HandleRequest(new IsEstablishmentAccessibleInScopeRequest(
                urn,
                EstablishmentScopeInfo.Create(scope, scopeId)
            ))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}