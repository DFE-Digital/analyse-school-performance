using ASP.Core.Configuration;
using ASP.Domain.DataDownloads;
using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;
using ASP.Domain.DataDownloads.UseCases.GetDownloadPackage;
using ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;
using ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;
using ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;
using ASP.Domain.Templating.UseCases.GetAllContentTemplates;
using ASP.Domain.Templating.UseCases.UpdateContentTemplate;
using ASP.Domain.Templating.UseCases.GetContentTemplate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.UseCases.GetAllSchools;
using ASP.Domain.Schools.UseCases.GetSchoolDetails;
using ASP.Domain.Schools.UseCases.GetLinkedSchools;
using ASP.Domain.Schools.UseCases.IsSchoolAccessibleInScope;
using ASP.Domain.Schools.UseCases.SchoolSearch;
using ASP.Domain.Schools.UseCases.SchoolSearchSuggestions;

namespace ASP.Domain;

public static class AspDomainExtensions
{
    public static IServiceCollection ConfigureContentTemplates(this IServiceCollection services)
    {
        services.AddScoped<IUpdateContentTemplate, UpdateContentTemplate>();
        services.AddScoped<IGetContentTemplate, GetContentTemplate>();
        services.AddScoped<IGetAllContentTemplates, GetAllContentTemplates>();

        return services;
    }

    public static IServiceCollection ConfigureSchools(this IServiceCollection services)
    {
        services.AddScoped<IGetAllSchoolsUseCase, GetAllSchoolsUseCase>();
        services.AddScoped<IGetSchoolDetailsUseCase, GetSchoolDetailsUseCase>();
        services.AddScoped<ISchoolSearchUseCase, SchoolSearchUseCase>();
        services.AddScoped<ISchoolSearchSuggestionsUseCase, SchoolSearchSuggestionsUseCase>();
        services.AddScoped<IIsSchoolAccessibleInScopeUseCase, IsSchoolAccessibleInScopeUseCase>();
        services.AddScoped<IGetLinkedSchoolsUseCase, GetLinkedSchoolsUseCase>();
        services.TryAddScoped<ISchoolAccessScopeValidator, SchoolAccessScope.Validator>();

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