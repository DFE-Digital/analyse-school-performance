using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetAllContentTemplates : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAllContentTemplates _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetAllContentTemplates(
            ILoggerFactory loggerFactory,
            IGetAllContentTemplates useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<GetAllContentTemplates>();
            _useCase = useCase ??
                throw new ArgumentNullException(nameof(useCase));
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetAllContentTemplates")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{req.Method} {req.Path + req.QueryString}");

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => _useCase.HandleRequest())
                .ToApiResultAsync(_options, cancellationToken);
        }
    }
}
