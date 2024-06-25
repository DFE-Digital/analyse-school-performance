using Microsoft.AspNetCore.Http;

namespace ASP.Api
{
    public abstract class ApiFunction
    {
        public abstract Task<ApiResult> Run(HttpRequest req, CancellationToken cancellationToken);
    }
}