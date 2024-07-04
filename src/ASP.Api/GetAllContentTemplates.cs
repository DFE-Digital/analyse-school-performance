using ASP.Application.UseCases.ContentPage.GetAllAllContentTemplates;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class GetAllContentTemplates : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IGetAllContentTemplatesUseCase _useCase;
        
        public GetAllContentTemplates(ILoggerFactory loggerFactory, IGetAllContentTemplatesUseCase useCase)
        {
            _logger = loggerFactory.CreateLogger<GetAllContentTemplates>();
            _useCase = useCase ??
                throw new ArgumentNullException(nameof(useCase));
        }


        [Function("GetAllContentTemplates")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{req.Method} {req.Path + req.QueryString}");

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => _useCase.HandleRequest())
                .ToApiResultAsync(cancellationToken);
        }
    }
}
