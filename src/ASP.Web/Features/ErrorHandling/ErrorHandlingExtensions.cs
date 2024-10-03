using ASP.Infrastructure;
using ASP.Web.Core.Environment;
using ASP.Web.Core.ErrorHandling;

namespace ASP.Web.Features.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        internal static IServiceCollection ConfigureErrorHandling(this IServiceCollection services, IConfiguration configuration, out ErrorHandlingOptions errorHandlingConfig)
        {
            services
                .ConfigureOptions(configuration, out errorHandlingConfig)
                .AddExceptionHandler<ExceptionLoggingMiddleware>();

            return services;
        }

        internal static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app, IWebHostEnvironment environment, ErrorHandlingOptions errorHandlingConfig)
        {
            // this must be called before the UseExceptionHandler so that it can pass the correct errors message through to the view
            // see CustomPageNotFoundMiddleware comments for how it works
            app.UseMiddleware<StatusCodePageLoggingMiddleware>();

            // we only want to call exception handling middleware during production or testing the production server error pages
            if (environment.ShouldUseProductionErrorPage(errorHandlingConfig))
            {
                app.UseExceptionHandler("/error/servererror/");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }
    }
}
