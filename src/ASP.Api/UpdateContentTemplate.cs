using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
namespace ASP.Api
{
    public class UpdateContentTemplate : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IUpdateContentTemplateUseCase _update;

        public UpdateContentTemplate(ILoggerFactory loggerFactory, IUpdateContentTemplateUseCase update)
        {
            _logger = loggerFactory.CreateLogger<UpdateContentTemplate>();
            _update = update;
        }

        [Function("UpdateContentTemplate")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            return await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Post])
                .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => RequestValidation.OptionalParameter(req, "revision")
                .Then(revision => RequestValidation.RequiredBodyAsync<ContentTemplate>(req)
                .Then(contentTemplate => _update.HandleRequest(new UpdateContentTemplateRequest(id, revision.ToNullable(), contentTemplate))))))
                .ToApiResultAsync();
        }
    }
}