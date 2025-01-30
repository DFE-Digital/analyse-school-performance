using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ASP.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration BindConfig<T>(this IConfiguration configuration, out T config)
            where T : class, new()
        {
            var sectionName = typeof(T).GetField("SectionName", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.GetRawConstantValue() is string s ? s : typeof(T).Name;
            var section = configuration.GetSection(sectionName);
            config = section.Get<T>() ?? new();

            return configuration;
        }

        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration, out T config)
            where T : class, new()
        {
            var sectionName = typeof(T).GetField("SectionName", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)?.GetRawConstantValue() is string s ? s : typeof(T).Name;
            var section = configuration.GetSection(sectionName);
            services.Configure<T>(section);
            config = section.Get<T>() ?? new();

            return services;
        }

        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            services.ConfigureOptions<T>(configuration, out var _);

            return services;
        }
    }
}
