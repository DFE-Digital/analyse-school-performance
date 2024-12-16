using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions;

public class GetAvailableLADownloads : ApiFunction
{
    private readonly ILogger<GetAvailableLADownloads> _logger;
    private readonly IGetAvailableLADownloads _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAvailableLADownloads(
        ILogger<GetAvailableLADownloads> logger,
        IGetAvailableLADownloads useCase,
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

    [Function("GetAvailableLADownloads")]
    [OpenApiOperation(operationId: "GetAvailableLADownloads", tags: ["Downloads"], Description = "Retrieves available downloads for specific local authority based on the code.")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Query, Required = true, Description = "The local authority code (3 digits).")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Query, Required = false, Description = "The requested year for downloads.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetAvailableLADownloadsResponse), Description = "Available local authority downloads for the specified code and year.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid parameters provided.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No available downloads for the specified code.")]
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
            from code in request.ValidateParameter("code", p => p.IsRequired().IsDigits().HasLength(3))
            from year in request.ValidateParameter("year", p => p.IsOptional().HasLength(4).IsNumeric())
            from response in _useCase.HandleRequest(new GetAvailableLADownloadsRequest(code, year))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}