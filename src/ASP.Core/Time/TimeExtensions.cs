using Microsoft.Extensions.DependencyInjection;

namespace ASP.Core.Time
{
    public static class TimeExtensions
    {
        public static IServiceCollection ConfigureCurrentTime(this IServiceCollection services)
        {
            services.AddSingleton<CurrentTimeProvider>();

            return services;
        }
    }
}