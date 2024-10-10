using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.GetAllContentTemplates;
using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using Microsoft.Extensions.DependencyInjection;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;

namespace ASP.Application;

public static class AspApplicationExtensions
{
    public static IServiceCollection RegisterUseCases(this IServiceCollection services)
    {
        services.AddScoped<IUpdateContentTemplate, UpdateContentTemplate>();
        services.AddScoped<IViewContentTemplate, ViewContentTemplate>();
        services.AddScoped<IGetAllEstablishments, GetAllEstablishments>();
        services.AddScoped<IGetEstablishmentDetails, GetEstablishmentDetails>();
        services.AddScoped<IEstablishmentSearch, EstablishmentSearch>();
        services.AddScoped<IEstablishmentSearchSuggestions, EstablishmentSearchSuggestions>();
        services.AddScoped<IGetAllContentTemplates, GetAllContentTemplates>();
        services.AddScoped<IGetAvailableLADownloads, GetAvailableLADownloads>();
        services.AddScoped<IGetLocalAuthority, GetLocalAuthority>();
        services.AddScoped<IGetAllLocalAuthorities, GetAllLocalAuthorities>();
        services.AddScoped<IGetAvailableSchoolDownloads, GetAvailableSchoolDownloads>();
        services.AddScoped<IGetMultiAcademyTrust, GetMultiAcademyTrust>();
        services.AddScoped<IDownloadAsZipFile, DownloadAsZipFile>();
        
        return services;
    }
}