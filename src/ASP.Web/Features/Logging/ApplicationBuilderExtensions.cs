namespace ASP.Web.Features.Logging
{
    public static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<PageNotFoundLoggingMiddleware>();

            return app;
        }
    }
}