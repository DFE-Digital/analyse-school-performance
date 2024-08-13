using ASP.Core.Establishments.Search;
using ASP.Core.MultiAcademyTrusts;
using ASP.Infrastructure.Establishments;
using ASP.Infrastructure.MultiAcademyTrusts;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSearchServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, EstablishmentNameOrLocationSearchService>();
        return services;
    }
    
    public static IServiceCollection ConfigureMultiAcademyTrustServices(this IServiceCollection services)
    {
        services.AddScoped<IMultiAcademyTrustRepository, MultiAcademyTrustRepository>();

        return services;
    }
}