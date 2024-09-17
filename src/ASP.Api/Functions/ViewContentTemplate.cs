using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Core.Results;
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
        public override async Task<ApiResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] 
            HttpRequest request, 
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(request.Method + " " + request.Path + request.QueryString);

            var result =
                from _ in request.ValidateHttpMethod([HttpMethods.Get])
                from id in request.ValidateParameter("id", p => p.IsRequired())
                from revision in request.ValidateParameter("revision", p => p.IsOptional())
                from response in _view.HandleRequest(new ViewContentTemplateRequest(id, revision))
                select response;

            return await result.ToApiResultAsync(_options, cancellationToken);
        }
    }
}
