namespace ASP.Web.Features.ContentSecurityPolicy
{
    public class ContentSecurityPolicyMiddleware
    {
        private readonly RequestDelegate _next;

        public ContentSecurityPolicyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var nonce = context.RequestServices.GetService<INonceService>();
            if(nonce is null)
            {
                return;
            }

            context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self'" + "; script-src " + $"'nonce-{nonce.GetNonce()}'" + "; style-src " + $"'nonce-{nonce.GetNonce()}'");

            await _next.Invoke(context);
        }
    }
}
