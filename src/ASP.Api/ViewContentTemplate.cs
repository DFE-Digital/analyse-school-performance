using ASP.Application.UseCases.ViewContentTemplate;
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
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            if (req.Method != "GET")
            {
                return new ApiResult(405, $"The HTTP method {req.Method} is not allowed.") { Headers = { { "Allow", "GET" } } };
            }

            var id = req.Query["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                return new ApiResult(400, @"Missing parameter: ""id"".");
            }

            var response = await _view.HandleRequest(new ViewContentTemplateRequest(id));

            return response.MatchFirst(r => new ApiResult(200, r), e =>
            {
                var statusCode = e.Code switch {
                    "General.NotFound" => 404,
                    _ => 500
                };

                return new ApiResult(statusCode, e.Description);
            });
        }
    }
}
