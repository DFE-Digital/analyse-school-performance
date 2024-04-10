using ASP.Web.Features.ContentSecurityPolicy;
using ASP.Web.Features.ErrorHandling;
using ASP.Web.Features.Logging;

namespace ASP.Web.Features
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFeatures(this IApplicationBuilder app, IWebHostEnvironment environment)
        {

            return app;
        }
    }
}