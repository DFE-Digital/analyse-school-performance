using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace ASP.Api.Functions
{
    public class UpdateContentTemplate : ApiFunction
    {
        private readonly ILogger _logger;
        private readonly IUpdateContentTemplate _update;
        private readonly ErrorHandlingOptions _options;

        public UpdateContentTemplate(
            ILoggerFactory loggerFactory,
            IUpdateContentTemplate update,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = loggerFactory.CreateLogger<UpdateContentTemplate>();
            _update = update;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [Function("UpdateContentTemplate")]
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            _logger.LogInformation(req.Method + " " + req.Path + req.QueryString);

            var apiResult = await RequestValidation.RequiredHttpMethod(req, [HttpMethods.Post])
                .Then(_ => RequestValidation.RequiredParameter(req, "id")
                .Then(id => RequestValidation.OptionalParameter(req, "revision")
                .Then(revision => RequestValidation.RequiredBodyAsync<ContentTemplate>(req)
                .Then(contentTemplate => _update.HandleRequest(new UpdateContentTemplateRequest(id, revision, contentTemplate))))))
                .ToApiResultAsync(_options, cancellationToken);

            return apiResult;
        }
    }
}