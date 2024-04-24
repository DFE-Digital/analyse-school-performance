using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Templating;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

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
        public override async Task<ApiResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", "get")] HttpRequest req)
        {
            if (req.Method != HttpMethods.Post)
            {
                return new ApiResult(405, $"The HTTP method {req.Method} is not allowed.") { Headers = { { "Allow", "POST" } } };
            }

            StringValues id = req.Query["id"];
            if (StringValues.IsNullOrEmpty(id))
            {
                return new ApiResult(400, @"Missing parameter: ""id"".");
            }


            using (var sr = new StreamReader(req.Body))
            {
                string content = await sr.ReadToEndAsync();
                if (content.Length == 0)
                    return new ApiResult(400, "Missing request body.");


                return await JsonHelper.DeserializeIgnoringMissingMembers<ContentTemplate>(content)
                    .MapError(e => {
                        return Error.Validation("Request body was not a JSON object.");
                    })
                    .ThenAsync(async pageContent => await _update.HandleRequest(new UpdateContentTemplateRequest(id.ToString() ?? "", pageContent)))
                    .Match(r => new ApiResult(200, r), e =>
                    {
                        int statusCode = e switch {
                            NotFoundError => 404,
                            ValidationError => 400,
                            _ => 500
                        };

                        return new ApiResult(statusCode, e.Message);
                    });
            }
        }
    }
}
