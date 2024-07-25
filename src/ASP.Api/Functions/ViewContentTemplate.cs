using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Api.Functions
{
    public class ViewContentTemplate : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IViewContentTemplate _view;
        private readonly ErrorHandlingOptions _options;

        public ViewContentTemplate(
            ILoggerFactory loggerFactory,
            IViewContentTemplate view,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<ViewContentTemplate>();
            _view = view;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("ViewContentTemplate")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            var apiResult = await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Get])
                .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => RequestValidation.OptionalParameter(req, "revision")
                .Then(revision => _view.HandleRequest(new ViewContentTemplateRequest(id, revision)))))
                .ToApiResultAsync(_options, cancellationToken);

            return apiResult;
        }
    }
}
