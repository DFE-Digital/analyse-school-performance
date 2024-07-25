using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class EstablishmentSearchSuggestions : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearchSuggestions _useCase;
        private readonly ErrorHandlingOptions _options;

        public EstablishmentSearchSuggestions(
            ILoggerFactory loggerFactory,
            IEstablishmentSearchSuggestions useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearchSuggestions>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("EstablishmentSearchSuggestions")]
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "searchTerm")
                    .Then(searchTerm => RequestValidation.OptionalParameter(req, "maxSuggestions")
                        .Then(maxSuggestions => RequestValidation.NumericParameter(maxSuggestions, "maxSuggestions"))
                        .Then(maxSuggestions => _useCase.HandleRequest(
                            new EstablishmentSearchSuggestionsRequest(searchTerm, maxSuggestions)))))
                .ToApiResultAsync(_options, cancellationToken);
        }
    }
}