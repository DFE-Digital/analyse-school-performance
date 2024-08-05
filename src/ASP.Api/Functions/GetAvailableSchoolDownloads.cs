using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAvailableSchoolDownloads : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAvailableSchoolDownloads _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAvailableSchoolDownloads(
            ILoggerFactory loggerFactory, 
            IGetAvailableSchoolDownloads useCase,
            IOptions<ErrorHandlingOptions> options)
        {
            _logger = loggerFactory.CreateLogger<GetAvailableSchoolDownloads>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAvailableSchoolDownloads")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{req.Method} {req.Path + req.QueryString}");

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "urn"))
                .Then(urn => RequestValidation.RequiresParameterLengthToMatch(urn, "urn", 6))
                .Then(urn => RequestValidation.RequiresParameterToBeDigits(urn, "urn"))
                .Then(urn => _useCase.HandleRequest(new GetAvailableSchoolDownloadsRequest(urn)))
                .ToApiResultAsync(_options, cancellationToken);
        }
    }
}
