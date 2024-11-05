using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions;

public class GetAvailableSchoolDownloads : ApiFunction
{
    private readonly ILogger<GetAvailableSchoolDownloads> _logger;
    private readonly IGetAvailableSchoolDownloads _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAvailableSchoolDownloads(
        ILogger<GetAvailableSchoolDownloads> logger,
        IGetAvailableSchoolDownloads useCase,
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

    [Function("GetAvailableSchoolDownloads")]
    [OpenApiOperation(operationId: "GetAvailableSchoolDownloads", tags: ["Downloads"], Description = "Retrieves available downloads for specific school based on URN.")]
    [OpenApiParameter(name: "urn", In = ParameterLocation.Query, Required = true, Description = "The URN of the school.")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Query, Required = false, Description = "The requested year for downloads.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetAvailableSchoolDownloadsResponse), Description = "Available school downloads for the specified URN and year.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid parameters provided.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No available downloads for the specified URN.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation($"{request.Method} {request.Path + request.QueryString}");

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from urn in request.ValidateParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
            from year in request.ValidateParameter("year", p => p.IsOptional().HasLength(4).IsNumeric())
            from response in _useCase.HandleRequest(new GetAvailableSchoolDownloadsRequest(urn, year))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
