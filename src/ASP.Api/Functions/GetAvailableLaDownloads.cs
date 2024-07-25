using ASP.Application.UseCases.Downloads.LaDownloads;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAvailableLaDownloads : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetLaDownloadsUseCase _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAvailableLaDownloads(ILoggerFactory loggerFactory, IGetLaDownloadsUseCase useCase, IOptions<ErrorHandlingOptions> options)
        {
            _logger = loggerFactory.CreateLogger<GetAvailableLaDownloads>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAvailableLaDownloads")]
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]
            HttpRequest req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "laCode")
                .Then(laCode => _useCase.HandleRequest(new GetLaDownloadsUseCaseRequest(laCode))))
                 .ToApiResultAsync(_options, cancellationToken);
        }
    }
}