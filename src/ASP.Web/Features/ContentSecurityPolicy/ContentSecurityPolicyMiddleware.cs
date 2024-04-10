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
            var nonce = (INonceService)context.RequestServices.GetService(typeof(INonceService));

            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'" + "; script-src " + $"'nonce-{nonce.GetNonce()}'" + "; style-src " + $"'nonce-{nonce.GetNonce()}'");

            await _next.Invoke(context);
        }
    }
}
