using ASP.Core.Establishments;
using ASP.Infrastructure.Repositories;

namespace ASP.Web.Areas.School
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSchoolPages(this IServiceCollection services)
        {
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();

            return services;
        }
    }
}
