using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Application.UseCases.Downloads.LaDownloads;
using ASP.Core.Search.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUpdateContentTemplate, UpdateContentTemplate>();
        services.AddScoped<IViewContentTemplate, ViewContentTemplate>();
        services.AddScoped<IGetEstablishmentDetails, GetEstablishmentDetails>();
        services.AddScoped<IEstablishmentSearch, EstablishmentSearch>();
        services.AddScoped<IEstablishmentSearchSuggestions, EstablishmentSearchSuggestions>();
        services.AddScoped<IGetAllContentTemplates, GetAllContentTemplates>();
        services.AddScoped<IGetLaDownloadsUseCase, GetLaDownloadsUseCase>();

        return services;
    }

    public static IServiceCollection ConfigureSearchStrategyFactory(this IServiceCollection services)
    {
        // Register the factory
        services.AddScoped<IEstablishmentSearchStrategyFactory, EstablishmentSearchStrategyFactory>();

        return services;
    }
}