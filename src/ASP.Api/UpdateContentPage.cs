using ASP.Application.UseCases.UpdateContentPage;
using ASP.Core.Helpers;
using ASP.Core.PageContent;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ASP.Api
{
    public class UpdateContentPage
    {
        private readonly ILogger _logger;
        private readonly IUpdateContentPageUseCase _update;

        public UpdateContentPage(ILoggerFactory loggerFactory, IUpdateContentPageUseCase update)
        {
            _logger = loggerFactory.CreateLogger<UpdateContentPage>();
            _update = update;
        }

        [Function("UpdateContentPage")]
        public async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            if (req.Method != "POST")
            {
                return new ApiResult(405, $"The HTTP method {req.Method} is not allowed.") { Headers = { { "Allow", "POST" } } };
            }

            var id = req.Query["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                return new ApiResult(400, @"Missing parameter: ""id"".");
            }

            if (req.Body.Length == 0)
            {
                return new ApiResult(400, "Missing request body.");
            }

            using (var sr = new StreamReader(req.Body))
            {
                var content = await sr.ReadToEndAsync();
                return await JsonHelper.DeserializeIgnoringMissingMembers<PageContentTemplate>(content)
                    .Else(e => {
                        return Error.Validation(description: "Request body was not a JSON object.");
                    })
                    .ThenAsync(async pageContent => await _update.HandleRequest(new UpdateContentPageRequest(id, pageContent)))
                    .MatchFirst(r => new ApiResult(200, r), e =>
                    {
                        var statusCode = e.Code switch {
                            "General.NotFound" => 404,
                            "General.Validation" => 400,
                            _ => 500
                        };

                        return new ApiResult(statusCode, e.Description);
                    });
            }
        }
    }
}
