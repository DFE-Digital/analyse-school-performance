using ASP.Web.Middleware;
using ASP.Web.Services;

namespace ASP.Web.Middleware
{
    public class CspMiddleware
    {
        private readonly RequestDelegate _next;

        public CspMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var nonce = (INonceService)context.RequestServices.GetService(typeof(INonceService));

            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'" + "; script-src " + $"'nonce-{nonce.GetNonce()}'" + "; style-src " + $"'nonce-{nonce.GetNonce()}'");

            await _next.Invoke(context);
        }
    }
}

public static class CspExtensions
{
    public static IApplicationBuilder UseNonce(this IApplicationBuilder app)
    {
       return app.UseMiddleware<CspMiddleware>();
    }
}
