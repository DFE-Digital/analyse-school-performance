using ASP.Domain.DataDownloads;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;

namespace ASP.Api.Functions.DataDownloads;

public class GetAvailableDownloads : ApiFunction
{
    private readonly ILogger<GetAvailableDownloads> _logger;
    private readonly IGetAvailableDownloads _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetAvailableDownloads(
        ILogger<GetAvailableDownloads> logger,
        IGetAvailableDownloads useCase,
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

    [Function("GetAvailableDownloads")]
    [OpenApiOperation(operationId: "GetAvailableDownloads", tags: ["Downloads"], Description = "Retrieves available downloads for specific local authority based on the code.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = true, Description = "Scope of the available downloads, e.g., `School` or `LA`.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = true, Description = "An identifier for the selected scope, either a school URN or LA code")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Query, Required = false, Description = "The requested year for downloads.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetAvailableDownloadsResponse), Description = "Available local authority downloads for the specified code and year.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Invalid parameters provided.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Not found: No available downloads for the specified code.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "downloads")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsRequired().IsEnum<DataDownloadsScopeType>())
            from scopeIdentifier in request.ValidateQueryStringParameter("scopeId", p => p.IsRequired())
            from year in request.ValidateQueryStringParameter("year", p => p.IsOptional().HasLength(4).IsNumeric())
            from response in _useCase.HandleRequest(new GetAvailableDownloadsRequest(scope, scopeIdentifier, year))
            select response.ForApiClient();

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}