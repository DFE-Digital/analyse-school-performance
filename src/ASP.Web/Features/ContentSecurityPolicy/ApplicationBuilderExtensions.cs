namespace ASP.Web.Features.ContentSecurityPolicy
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                //not used in dev
                app.UseMiddleware<ContentSecurityPolicyMiddleware>();
            }

            return app;
        }
    }
}