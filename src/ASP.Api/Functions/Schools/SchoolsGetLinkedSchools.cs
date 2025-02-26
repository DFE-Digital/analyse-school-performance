using ASP.Api.Client.Schools;
using ASP.Core.Results;
using ASP.Domain.Schools.UseCases.GetLinkedSchools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Schools;

public class SchoolsGetLinkedSchools : ApiFunction
{

    private readonly ILogger<SchoolsGetLinkedSchools> _logger;
    private readonly IGetLinkedSchoolsUseCase _useCase;
    private readonly ApiResultConverter _resultConverter;

    public SchoolsGetLinkedSchools(
        ILogger<SchoolsGetLinkedSchools> logger,
        IGetLinkedSchoolsUseCase useCase,
        ApiResultConverter resultConverter)
    {
        _logger = logger
                  ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
                   ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
                           ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [Function("SchoolsGetLinkedSchools")]
    [OpenApiOperation(operationId: "SchoolsGetLinkedSchools", tags: ["Schools"], Description = "Retrieves linked schools for a specific school based on URN.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Path, Required = true, Description = "The URN of the school.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SchoolsGetLinkedSchoolsResponse), Description = "Details of the linked schools for the specified URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid URN parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find a school with URN {urn}.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "schools/{urn:int}/linked-schools")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidatePathParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from response in _useCase.HandleRequest(new GetLinkedSchoolsRequest(urn))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}