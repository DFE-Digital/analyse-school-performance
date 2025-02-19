using ASP.Core.Results;
using ASP.Domain.DataDownloads;
using ASP.Domain.DataDownloads.UseCases.GetDownloadPackage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace ASP.Api.Functions.Downloads;

public class DownloadsGetPackage : ApiFunction
{
    private readonly ILogger<DownloadsGetPackage> _logger;
    private readonly IGetDownloadPackage _useCase;
    private readonly ApiResultConverter _resultConverter;

    public DownloadsGetPackage(
        ILogger<DownloadsGetPackage> logger,
        IGetDownloadPackage useCase,
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


    [Function("DownloadsGetPackage")]
    [OpenApiOperation(operationId: "DownloadsGetPackage", tags: ["Downloads"], Description = "Creates a ZIP archive of multiple downloads.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = true, Description = "Scope of the downloads requested, e.g. 'LA' or 'School'.")]
    [OpenApiParameter(name: "scopeId", In = ParameterLocation.Query, Required = true, Description = "An identifier for the selected scope, either a school URN or LA code")]
    [OpenApiParameter(name: "fileType", In = ParameterLocation.Query, Required = true, Description = "Type of the file to be downloaded, examples: `csv`, `txt`, `xlsx`")]
    [OpenApiParameter(name: "downloadIds", In = ParameterLocation.Query, Required = true, Type = typeof(List<string>), Description = "List of file IDs to be downloaded as ZIP.", Explode = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/zip", bodyType: typeof(byte[]), Description = "The ZIP file containing the requested files.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Description = "Method not allowed: The HTTP method POST is not allowed.")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "downloads/package")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from scope in request.ValidateQueryStringParameter("scope", p => p.IsRequired().IsNotEmpty().IsEnum<DataDownloadsScopeType>())
            from scopeIdentifier in request.ValidateQueryStringParameter("scopeId", p => p.IsRequired().IsNotEmpty())
            from fileType in request.ValidateQueryStringParameter("fileType", p => p.IsRequired().IsNotEmpty().IsEnum<FileType>())
            from downloadIds in request.ValidateQueryStringParameter("downloadIds", p => p.IsRequiredMultiParameter().IsNotEmpty())
            from response in _useCase.HandleRequest(new GetDownloadPackageRequest(fileType, downloadIds, scope, scopeIdentifier))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
