using ASP.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Api
{
    public class ApiResult : ContentResult
    {
        public object Value { get; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public ApiResult(int statusCode, string message)
        {
            StatusCode = statusCode;
            Value = message;
            Content = message;
            ContentType = "text/plain";
        }

        public ApiResult(int statusCode, object value)
        {
            StatusCode = statusCode;
            Value = value;
            Content = JsonHelper.Serialize(value);
            ContentType = "application/json";
        }


        public override async Task ExecuteResultAsync(ActionContext context)
        {
            HttpResponse response = context.HttpContext.Response;
            foreach (var header in Headers)
            {
                response.Headers.Append(header.Key, header.Value);
            }

            await base.ExecuteResultAsync(context);
        }
    }
}
