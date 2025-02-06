using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ASP.Core.Results;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using ASP.Domain.DataDownloads;
using ASP.Domain.DataDownloads.UseCases.GetDownloadPackage;

namespace ASP.Api.Functions.DataDownloads;

public class GetDownloadPackage : ApiFunction
{
    private readonly ILogger<GetDownloadPackage> _logger;
    private readonly IGetDownloadPackage _useCase;
    private readonly ApiResultConverter _resultConverter;

    public GetDownloadPackage(
        ILogger<GetDownloadPackage> logger,
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


    [Function("GetDownloadPackage")]
    [OpenApiOperation(operationId: "GetDownloadPackage", tags: ["Downloads"], Description = "Creates a ZIP archive of multiple downloads.")]
    [OpenApiParameter(name: "scope", In = ParameterLocation.Query, Required = true, Description = "Scope of the request, e.g. 'LA' or 'School'.")]
    [OpenApiParameter(name: "scopeIdentifier", In = ParameterLocation.Query, Required = true, Description = "An identifier for the selected scope, either a school URN or LA code")]
    [OpenApiParameter(name: "fileType", In = ParameterLocation.Query, Required = true, Description = "Type of the file to be downloaded, examples: `csv`, `txt`, `xlsx`")]
    [OpenApiParameter(name: "downloadIds", In = ParameterLocation.Query, Required = true, Type = typeof(List<string>), Description = "List of file IDs to be downloaded as ZIP.", Explode = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/zip", bodyType: typeof(byte[]), Description = "The ZIP file containing the requested files.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Bad request: Missing or invalid parameters.")]
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
            from scope in request.ValidateParameter("scope", p => p.IsRequired().IsEnum<DataDownloadsScopeType>())
            from scopeIdentifier in request.ValidateParameter("scopeIdentifier", p => p.IsRequired())
            from fileType in request.ValidateParameter("fileType", p => p.IsRequired().IsEnum<FileType>())
            from downloadIds in request.ValidateParameter("downloadIds", p => p.IsRequiredMultiParameter())
            from response in _useCase.HandleRequest(new GetDownloadPackageRequest(fileType, downloadIds, scope, scopeIdentifier))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
