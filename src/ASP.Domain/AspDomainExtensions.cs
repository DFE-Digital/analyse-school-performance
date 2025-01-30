using ASP.Core.Configuration;
using ASP.Domain.DataDownloads;
using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;
using ASP.Domain.DataDownloads.UseCases.GetDownloadPackage;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.UseCases.EstablishmentSearch;
using ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;
using ASP.Domain.Establishments.UseCases.GetAllEstablishments;
using ASP.Domain.Establishments.UseCases.GetEstablishmentDetails;
using ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;
using ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;
using ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;
using ASP.Domain.Templating.UseCases.GetAllContentTemplates;
using ASP.Domain.Templating.UseCases.UpdateContentTemplate;
using ASP.Domain.Templating.UseCases.ViewContentTemplate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Domain;

public static class AspDomainExtensions
{
    public static IServiceCollection ConfigureContentTemplates(this IServiceCollection services)
    {
        services.AddScoped<IUpdateContentTemplate, UpdateContentTemplate>();
        services.AddScoped<IViewContentTemplate, ViewContentTemplate>();
        services.AddScoped<IGetAllContentTemplates, GetAllContentTemplates>();

        return services;
    }

    public static IServiceCollection ConfigureEstablishments(this IServiceCollection services)
    {
        services.AddScoped<IGetAllEstablishments, GetAllEstablishments>();
        services.AddScoped<IGetEstablishmentDetails, GetEstablishmentDetails>();
        services.AddScoped<IEstablishmentSearch, EstablishmentSearch>();
        services.AddScoped<IEstablishmentSearchSuggestions, EstablishmentSearchSuggestions>();
        services.TryAddScoped<IEstablishmentScopeValidator, EstablishmentScope.Validator>();

        return services;
    }

    public static IServiceCollection ConfigureLocalAuthorities(this IServiceCollection services)
    {
        services.AddScoped<IGetLocalAuthority, GetLocalAuthority>();
        services.AddScoped<IGetAllLocalAuthorities, GetAllLocalAuthorities>();
        services.AddScoped<ILocalAuthoritySearchSuggestions, LocalAuthoritySearchSuggestions>();
        services.AddScoped<ILocalAuthoritySearch, LocalAuthoritySearch>();

        return services;
    }

    public static IServiceCollection ConfigureMultiAcademyTrusts(this IServiceCollection services)
    {
        services.AddScoped<IGetMultiAcademyTrust, GetMultiAcademyTrust>();

        return services;
    }

    public static IServiceCollection ConfigureDataDownloads(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGetAvailableDownloads, GetAvailableDownloads>();
        services.AddScoped<IGetDownloadPackage, GetDownloadPackage>();
        services.ConfigureOptions<DataDownloadsOptions>(configuration);
        services.TryAddScoped<IDataDownloadsScopeValidator, DataDownloadsScope.Validator>();

        return services;
    }
}