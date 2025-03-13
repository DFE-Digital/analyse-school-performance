using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        public static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}
