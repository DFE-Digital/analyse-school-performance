using ASP.Domain.DataDownloads;
using ASP.Domain.Establishments;
using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Domain.Templating;
using ASP.Domain.Repositories.DataDownloads;
using ASP.Domain.Repositories.Establishments;
using ASP.Domain.Repositories.LocalAuthorities;
using ASP.Domain.Repositories.MultiAcademyTrusts;
using ASP.Domain.Repositories.Templating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Domain.Repositories
{
    public static class AspDomainRepositoriesExtensions
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
