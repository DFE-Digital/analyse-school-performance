using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class EstablishmentSearchSuggestions : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearchSuggestionsUseCase _useCase;

        public EstablishmentSearchSuggestions(ILoggerFactory loggerFactory, IEstablishmentSearchSuggestionsUseCase useCase)
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearchSuggestions>();
            _useCase = useCase;
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
                            .Then(maxSuggestions =>
                                RequestValidation.NumericParameter(maxSuggestions, "maxSuggestions"))
                            .Then(maxSuggestions => _useCase.HandleRequest(
                                new EstablishmentSearchSuggestionsUseCaseRequest(searchTerm,
                                    RequestValidation.GetNumericParameterOrDefault(maxSuggestions,
                                        10)
                                )))))
                .ToApiResultAsync(cancellationToken);
        }
    }
}