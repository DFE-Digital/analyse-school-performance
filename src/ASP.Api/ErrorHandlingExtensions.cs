using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ASP.Infrastructure;

namespace ASP.Api
{
    public static class ErrorHandlingExtensions
    {
        internal static IServiceCollection ConfigureErrorHandling(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureOptions<ErrorHandlingOptions>(configuration);

            return services;
        }

        public static IFunctionsWorkerApplicationBuilder UseErrorHandling(this IFunctionsWorkerApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
