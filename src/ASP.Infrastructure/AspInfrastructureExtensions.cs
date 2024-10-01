using ASP.Core.Establishments;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Templating;
using ASP.Infrastructure.Establishments;
using ASP.Infrastructure.LocalAuthorities;
using ASP.Infrastructure.MultiAcademyTrusts;
using ASP.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure
{
    public static class AspInfrastructureExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IContentTemplateRepository, ContentTemplateRepository>();
            services.TryAddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.TryAddScoped<IMultiAcademyTrustRepository, MultiAcademyTrustRepository>();
            services.TryAddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();

            return services;
        }

        public static IConfiguration BindConfig<T>(this IConfiguration configuration, out T config)
            where T : class, new()
        {
            var sectionName = typeof(T).GetField("SectionName")?.GetRawConstantValue() is string s ? s : typeof(T).Name;
            var section = configuration.GetSection(sectionName);
            config = section.Get<T>() ?? new();

            return configuration;
        }

        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration, out T config)
            where T : class, new()
        {
            var sectionName = typeof(T).GetField("SectionName")?.GetRawConstantValue() is string s ? s : typeof(T).Name;
            var section = configuration.GetSection(sectionName);
            services.Configure<T>(section);
            config = section.Get<T>() ?? new();

            return services;
        }

        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class, new()
        {
            ConfigureOptions<T>(services, configuration, out var _);

            return services;
        }
    }
}
