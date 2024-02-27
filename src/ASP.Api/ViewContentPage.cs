using ASP.Application.UseCases.ViewContentPage;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class ViewContentPage
    {
        private readonly ILogger _logger;
        private readonly IViewContentPageUseCase _view;

        public ViewContentPage(ILoggerFactory loggerFactory, IViewContentPageUseCase view)
        {
            _logger = loggerFactory.CreateLogger<ViewContentPage>();
            _view = view;
        }
        
        [Function("ViewContentPage")]
        public async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
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

            var response = await _view.HandleRequest(new ViewContentPageRequest(id));

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
