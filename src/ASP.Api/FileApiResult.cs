using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Api
{
    public class FileApiResult : FileStreamResult
    {
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public FileApiResult(Stream fileStream, string contentType, string fileDownloadName)
            : base(fileStream, contentType)
        {
            FileDownloadName = fileDownloadName;
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
