using ASP.Web.Core.Environment;
using Microsoft.AspNetCore.CookiePolicy;

namespace ASP.Web.Features.ContentSecurityPolicy
{
    public static class ContentSecurityPolicyExtensions
    {
        public static IServiceCollection ConfigureContentSecurityPolicy(this IServiceCollection services)
        {
            services.AddScoped<INonceService>(serviceProvider => new NonceService(32));

            return services;
        }

        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (!(environment.IsDevelopment() || environment.IsLocalDevelopment()))
            {
                //not used in dev
                app.UseMiddleware<ContentSecurityPolicyMiddleware>();
            }

            return app;
        }

        // Add same-site Content-Security Policy for cookies
        // Add this before any other middleware that might write cookies
        public static IApplicationBuilder UseCookieContentSecurityPolicy(this IApplicationBuilder app)
        {
            app.UseCookiePolicy(new CookiePolicyOptions {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            return app;
        }
    }
}