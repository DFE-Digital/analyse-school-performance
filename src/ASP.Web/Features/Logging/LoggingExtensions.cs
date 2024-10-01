namespace ASP.Web.Features.Logging
{
    public static class LoggingExtensions
    {
        internal static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}
