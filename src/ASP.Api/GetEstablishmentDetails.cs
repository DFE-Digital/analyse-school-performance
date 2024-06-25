using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class GetEstablishmentDetails : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetEstablishmentDetailsUseCase _useCase;

        public GetEstablishmentDetails(ILoggerFactory loggerFactory, IGetEstablishmentDetailsUseCase useCase)
        {
            _logger = loggerFactory.CreateLogger<GetEstablishmentDetails>();
            _useCase = useCase;
        }
        
        [Function("GetEstablishmentDetails")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "urn")
                .Then(urn => _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn))))
                .ToApiResultAsync(cancellationToken);
        }
    }
}
