using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Core.Search.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureSearchStrategyFactory(this IServiceCollection services)
    {
        // Register the factory
        services.AddScoped<IEstablishmentSearchStrategyFactory, EstablishmentSearchStrategyFactory>();

        return services;
    }
   
}