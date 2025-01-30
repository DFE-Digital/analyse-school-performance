using ASP.Domain.DataDownloads;
using ASP.Domain.Establishments;
using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Domain.Templating;
using ASP.Infrastructure.Repositories.DataDownloads;
using ASP.Infrastructure.Repositories.Establishments;
using ASP.Infrastructure.Repositories.LocalAuthorities;
using ASP.Infrastructure.Repositories.MultiAcademyTrusts;
using ASP.Infrastructure.Repositories.Templating;
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
            services.TryAddScoped<IDataDownloadsFileProvider, BlobStorageDataDownloadsFileProvider>();

            return services;
        }
    }
}
