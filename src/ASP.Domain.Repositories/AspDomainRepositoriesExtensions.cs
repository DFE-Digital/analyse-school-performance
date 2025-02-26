using ASP.Domain.DataDownloads;
using ASP.Domain.LocalAuthorities;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Domain.Templating;
using ASP.Domain.Repositories.DataDownloads;
using ASP.Domain.Repositories.LocalAuthorities;
using ASP.Domain.Repositories.MultiAcademyTrusts;
using ASP.Domain.Repositories.Templating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Domain.Repositories.Schools;
using ASP.Domain.Schools;

namespace ASP.Domain.Repositories
{
    public static class AspDomainRepositoriesExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IContentTemplateRepository, ContentTemplateRepository>();
            services.TryAddScoped<ISchoolRepository, SchoolRepository>();
            services.TryAddScoped<IMultiAcademyTrustRepository, MultiAcademyTrustRepository>();
            services.TryAddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();
            services.TryAddScoped<IDataDownloadsFileProvider, BlobStorageDataDownloadsFileProvider>();

            return services;
        }
    }
}
