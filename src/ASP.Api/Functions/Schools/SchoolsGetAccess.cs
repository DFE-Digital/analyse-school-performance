using ASP.Api.Client.Schools;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Schools;

public class SchoolsGetAccess : ApiFunction
{
    private readonly ILogger<SchoolsGetAll> _logger;
    private readonly IIsSchoolAccessibleInScopeUseCase _useCase;
    private readonly ApiResultConverter _resultConverter;

    public SchoolsGetAccess(
        ILogger<SchoolsGetAll> logger,
        IIsSchoolAccessibleInScopeUseCase useCase,
        ApiResultConverter resultConverter)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
            ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("SchoolsGetAccess")]
    [OpenApiOperation(operationId: "SchoolsGetAccess", tags: ["Schools"], Description = "Determines whether a school is accessible within a specified scope.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Path, Required = true, Description = "The URN of the school.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = false, Description = "Scope to check to determine the schools's accessibility, e.g., `LA`, `MAT` or `Diocese`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = false, Description = "An identifier for the selected scope, e.g, LA code, MAT UID or Diocese name")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SchoolsGetAccessResponse), Description = "A response object indicating whether the school is accessible within the given scope.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any schools within the given scope.")]
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
            from urn in request.ValidatePathParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsOptional().IsNotEmpty().IsEnum<SchoolAccessScopeType>())
            from scopeId in request.ValidateQueryStringParameter("scopeId", p => p.IsRequiredIf(scope.HasValue))
            from response in _useCase.HandleRequest(new IsSchoolAccessibleInScopeRequest(
                urn,
                SchoolAccessScopeInfo.Create(scope, scopeId)
            ))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}