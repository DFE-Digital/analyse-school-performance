using ASP.Core.Search;
using ASP.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSearchServices(this IServiceCollection services)
    {
        services.AddScoped<ISearchService, EstablishmentNameOrLocationSearchService>();
        return services;
    }
}