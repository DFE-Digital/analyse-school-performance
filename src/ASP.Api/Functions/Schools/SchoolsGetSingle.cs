using ASP.Api.Client.Schools;
using ASP.Core.Results;
using ASP.Domain.Schools.UseCases.GetSchoolDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Schools;

public class SchoolsGetSingle : ApiFunction
{
    private readonly ILogger<SchoolsGetSingle> _logger;
    private readonly IGetSchoolDetailsUseCase _useCase;
    private readonly ApiResultConverter _resultConverter;

    public SchoolsGetSingle(
        ILogger<SchoolsGetSingle> logger,
        IGetSchoolDetailsUseCase useCase,
        ApiResultConverter resultConverter)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _useCase = useCase
            ?? throw new ArgumentNullException(nameof(useCase));

        _resultConverter = resultConverter
            ?? throw new ArgumentNullException(nameof(resultConverter));
    }

    [OpenApiOperation(operationId: "SchoolsGetSingle", tags: ["Schools"], 
        Description = "Retrieves details for a specific school based on a given URN.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Path, Required = true, 
        Description = "The URN of the school.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SchoolDetails), 
        Description = "Details of the school with the given URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, 
        Description = "Bad request: Invalid URN parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, 
        Description = "Not found: No school found for the given URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, 
        Description = "Method not allowed: The HTTP method POST/PUT/DELETE is not allowed.")]
    
    [Function("SchoolsGetSingle")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "put", "post", "delete", Route = "schools/{urn:int}")]
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidatePathParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from response in _useCase.HandleRequest(new GetSchoolDetailsRequest(urn))
            select response.ForApiClientAsDetails();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}