using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ASP.Core.Results;
using ASP.Application.UseCases.BlobStorageDemoZipFileDownload;

namespace ASP.Api.Functions;

public class BlobStorageDemoZipFileDownload : ApiFunction
{
    private readonly ILogger<BlobStorageDemoZipFileDownload> _logger;
    private readonly IBlobStorageDemoZipFileDownload _useCase;
    private readonly ApiResultConverter _resultConverter;

    public BlobStorageDemoZipFileDownload(
        ILogger<BlobStorageDemoZipFileDownload> logger,
        IBlobStorageDemoZipFileDownload useCase,
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

    [Function("BlobStorageDemoZipFileDownload")]
    public override async Task<ActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
        HttpRequest request,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

        var result =
            from _ in request.ValidateHttpMethod([HttpMethods.Get])
            from container in request.ValidateParameter("container", p => p.IsRequired())
            from filepath in request.ValidateParameter("filepath", p => p.IsRequired())
            from response in _useCase.HandleRequest(new BlobStorageDemoZipFileDownloadRequest(container, filepath))
            select response;

        return await _resultConverter.ConvertToApiResultAsync(result, cancellationToken);
    }
}
