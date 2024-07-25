using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class GetEstablishmentDetails : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetEstablishmentDetails _useCase;
        private readonly ErrorHandlingOptions _options;

        public GetEstablishmentDetails(
            ILoggerFactory loggerFactory,
            IGetEstablishmentDetails useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<GetEstablishmentDetails>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("GetEstablishmentDetails")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "urn")
                .Then(urn => _useCase.HandleRequest(new GetEstablishmentDetailsRequest(urn))))
                .ToApiResultAsync(_options, cancellationToken);
        }
    }
}
