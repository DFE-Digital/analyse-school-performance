using ASP.Web.Features.Logging;

namespace ASP.Web.Features.ErrorHandling
{
    public static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseMiddleware<PageNotFoundLoggingMiddleware>();

            if (environment.IsDevelopment())
            {
                app.UseMiddleware<CustomPageNotFoundMiddleware>();
            }
            else
            {
                // Handle status code exceptions like page not found
                app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");
            }
            app.UseExceptionHandler("/error");

            return app;
        }
    }
}