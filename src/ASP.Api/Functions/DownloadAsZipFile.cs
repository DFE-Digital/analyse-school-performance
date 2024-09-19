using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ASP.Core.Results;

namespace ASP.Api.Functions
{
    public class DownloadAsZipFile : ApiFunction
    {
        private readonly ILogger<DownloadAsZipFile> _logger;
        private readonly ErrorHandlingOptions _options;
        private readonly IDownloadAsZipFile _useCase;

        public DownloadAsZipFile(
            ILogger<DownloadAsZipFile> logger,
            IOptions<ErrorHandlingOptions> options,
            IDownloadAsZipFile useCase)
        {
            _logger = logger;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
            _useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        }


        [Function("DownloadAsZipFile")]
        public override async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from fileType in request.ValidateParameter("fileType", p => p.IsRequired().IsEnum<FileType>())
                from downloadIds in request.ValidateParameter("downloadIds", p => p.IsRequiredMultiParameter())
                from response in _useCase.HandleRequest(new DownloadAsZipFileRequest(fileType, downloadIds))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}
