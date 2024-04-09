using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

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
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            if (req.Method != "GET")
            {
                return new ApiResult(405, $"The HTTP method {req.Method} is not allowed.") { Headers = { { "Allow", "GET" } } };
            }

            var id = req.Query["id"];

            if (StringValues.IsNullOrEmpty(id))
            {
                return new ApiResult(400, @"Missing parameter: ""id"".");
            }

            var response = await _view.HandleRequest(new ViewContentTemplateRequest(id.ToString() ?? ""));

            return response.Match(r => new ApiResult(200, r), e =>
            {
                var statusCode = e switch {
                    NotFoundError => 404,
                    _ => 500
                };

                return new ApiResult(statusCode, e.Message);
            });
        }
    }
}
