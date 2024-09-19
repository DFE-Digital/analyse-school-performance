using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public override async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequest request, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Post])
                from id in request.ValidateParameter("id", p => p.IsRequired())
                from revision in request.ValidateParameter("revision", p => p.IsOptional())
                from contentTemplate in request.ValidateBodyAsync<ContentTemplate>()
                from response in _update.HandleRequest(new UpdateContentTemplateRequest(id, revision, contentTemplate))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}