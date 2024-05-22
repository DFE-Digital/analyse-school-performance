using Microsoft.AspNetCore.Rewrite;

namespace ASP.Web.Features.UrlRewriting
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUrlRewriteRules(this IApplicationBuilder app)
        {
            app.UseRewriter(new RewriteOptions()
                .Add(UrlRewriter.EnsureTrailingSlashOnSubdirectory));

            return app;
        }
    }
}