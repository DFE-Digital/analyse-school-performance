using ASP.Web.Middleware;

namespace ASP.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomPageNotFound(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return app.UseMiddleware<CustomPageNotFoundMiddleware>();
        }

        return app;
    }
    
    public static IApplicationBuilder UsePageNotFoundLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<PageNotFoundLoggingMiddleware>();
    }
}