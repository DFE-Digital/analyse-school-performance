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
    [OpenApiOperation(operationId: "GetAllLocalAuthorities", tags: ["Downloads"], Description = "Retrieves a paginated list of all local authority downloads.")]
    [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = false, Description = "The page number for pagination.")]
    [OpenApiParameter(name: "resultsPerPage", In = ParameterLocation.Query, Required = false, Description = "The number of results to return per page.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetAvailableLADownloadsResponse), Description = "A paginated list of local authorities.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid parameters provided.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: Could not find any local authorities.")]
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
            from laCode in request.ValidateParameter("laCode", p => p.IsRequired().IsDigits().HasLength(3))
            from year in request.ValidateParameter("year", p => p.IsOptional().HasLength(4).IsNumeric())
            from response in _useCase.HandleRequest(new GetAvailableLADownloadsRequest(laCode, year))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}