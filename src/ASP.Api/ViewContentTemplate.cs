using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class ViewContentTemplate : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IViewContentTemplateUseCase _view;

        public ViewContentTemplate(ILoggerFactory loggerFactory, IViewContentTemplateUseCase view)
        {
            _logger = loggerFactory.CreateLogger<ViewContentTemplate>();
            _view = view;
        }
        
        [Function("ViewContentTemplate")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function)] HttpRequest req)
        {
            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => RequestValidation.NotEmpty(id, "id"))
                .Then(id => _view.HandleRequest(new ViewContentTemplateRequest(id))))
                .ToApiResultAsync();
        }
    }
}
