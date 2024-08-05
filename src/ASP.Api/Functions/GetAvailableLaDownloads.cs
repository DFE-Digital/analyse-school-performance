using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAvailableLADownloads : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAvailableLADownloads _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAvailableLADownloads(ILoggerFactory loggerFactory, IGetAvailableLADownloads useCase, IOptions<ErrorHandlingOptions> options)
        {
            _logger = loggerFactory.CreateLogger<GetAvailableLADownloads>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAvailableLADownloads")]
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "laCode")
                .Then(laCode => _useCase.HandleRequest(new GetAvailableLADownloadsRequest(laCode))))
                 .ToApiResultAsync(_options, cancellationToken);
        }
    }
}