using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Results;
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
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] 
            HttpRequest request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from urn in request.ValidateParameter("urn", p => p.IsRequired().IsDigits().HasLength(6))
                from response in _useCase.HandleRequest(new GetEstablishmentDetailsRequest(urn))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}