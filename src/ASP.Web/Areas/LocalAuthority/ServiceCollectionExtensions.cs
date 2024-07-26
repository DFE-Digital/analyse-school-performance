using ASP.Core.LocalAuthority;
using ASP.Infrastructure.Repositories;

namespace ASP.Web.Areas.LocalAuthority
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureLocalAuthorityPages(this IServiceCollection services)
        {
            services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();

            return services;
        }
    }
}
