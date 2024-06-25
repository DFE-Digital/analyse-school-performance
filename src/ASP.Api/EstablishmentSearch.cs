using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class EstablishmentSearch : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IEstablishmentSearchUseCase _useCase;

        public EstablishmentSearch(ILoggerFactory loggerFactory, IEstablishmentSearchUseCase useCase)
        {
            _logger = loggerFactory.CreateLogger<EstablishmentSearch>();
            _useCase = useCase;
        }
        
        [Function("EstablishmentSearch")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "searchTerm")
                .Then(searchTerm => RequestValidation.RequiredParameter(req, "page")
                .Then(page => _useCase.HandleRequest(new EstablishmentSearchUseCaseRequest(searchTerm, 1 /*page*/)))))
                .ToApiResultAsync(cancellationToken);
        }
    }
}
