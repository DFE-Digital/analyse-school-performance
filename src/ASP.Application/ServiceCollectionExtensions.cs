using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using Microsoft.Extensions.DependencyInjection;
using ASP.Core.Establishments.Search;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;

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
        services.AddScoped<IGetAvailableLADownloads, GetAvailableLADownloads>();
        services.AddScoped<IGetLocalAuthority, GetLocalAuthority>();
        services.AddScoped<IGetAvailableSchoolDownloads, GetAvailableSchoolDownloads>();

        return services;
    }

    public static IServiceCollection ConfigureSearchStrategyFactory(this IServiceCollection services)
    {
        // Register the factory
        services.AddScoped<IEstablishmentSearchStrategyFactory, EstablishmentSearchStrategyFactory>();

        return services;
    }
}