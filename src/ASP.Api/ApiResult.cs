using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Api
{
    public class ApiResult : ObjectResult
    {
        public ApiResult(int statusCode, string message) 
            : base(message)
        {
            StatusCode = statusCode;
        }

        public ApiResult(int statusCode, object value) 
            : base(value)
        {
            StatusCode = statusCode;
        }

        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var header in Headers)
            {
                context.HttpContext?.Response?.Headers?.Append(header.Key, header.Value);
            }

            await base.ExecuteResultAsync(context);
        }
    }
}
