using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Core.Helpers;
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
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => RequestValidation.OptionalParameter(req, "revision")
                .Then(revision => _view.HandleRequest(new ViewContentTemplateRequest(id, revision.ToNullable())))))
                .ToApiResultAsync(cancellationToken);
        }
    }
}
