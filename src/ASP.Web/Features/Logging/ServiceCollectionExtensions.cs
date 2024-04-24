namespace ASP.Web.Features.Logging
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureLogging(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            return services;
        }
    }
}
