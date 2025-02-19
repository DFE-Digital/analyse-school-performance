using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        private static readonly ConfigurationHelper _helper = new();

        public static IConfiguration BindConfig<T>(this IConfiguration configuration, out T config)
            where T : class, new()
        {
            config = _helper.BindConfiguration<T>(configuration);
            return configuration;
        }

        public static IServiceCollection ConfigureOptions<T>(
            this IServiceCollection services,
            IConfiguration configuration)
            where T : class, new()
        {
            return ConfigureOptions<T>(services, configuration, out _);
        }

        public static IServiceCollection ConfigureOptions<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            out T config)
            where T : class, new()
        {
            _helper.ConfigureServices(services, configuration, out config);
            return services;
        }
    }
}