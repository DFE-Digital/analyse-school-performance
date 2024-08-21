using ASP.Core.Establishments.Search;
using ASP.Infrastructure.Establishments;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure;

public static class AspInfrastructureExtensions
{
    public static IServiceCollection ConfigureSearchServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, EstablishmentNameOrLocationSearchService>();
        return services;
    }
}