using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Core.Helpers;
using ASP.Core.Templating;
using ErrorOr;
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
                return await JsonHelper.DeserializeIgnoringMissingMembers<ContentTemplate>(content)
                    .Else(e => {
                        return Error.Validation(description: "Request body was not a JSON object.");
                    })
                    .ThenAsync(async pageContent => await _update.HandleRequest(new UpdateContentTemplateRequest(id, pageContent)))
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
