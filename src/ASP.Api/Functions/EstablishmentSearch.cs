using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class EstablishmentSearch : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearch _useCase;
        private readonly ErrorHandlingOptions _options;

        public EstablishmentSearch(
            ILoggerFactory loggerFactory,
            IEstablishmentSearch useCase,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearch>();
            _useCase = useCase;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("EstablishmentSearch")]
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "searchTerm")
                    .Then(searchTerm => RequestValidation.OptionalParameter(req, "page")
                        .Then(page => RequestValidation.NumericParameter(page, "page"))
                        .Then(page => RequestValidation.OptionalParameter(req, "resultsPerPage")
                            .Then(resultsPerPage => RequestValidation.NumericParameter(resultsPerPage, "resultsPerPage"))
                            .Then(resultsPerPage => _useCase.HandleRequest(
                                new EstablishmentSearchRequest(searchTerm, page, resultsPerPage))))))
                .ToApiResultAsync(_options, cancellationToken);
        }
    }
}