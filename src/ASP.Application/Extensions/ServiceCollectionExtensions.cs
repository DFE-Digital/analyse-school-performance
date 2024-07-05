using ASP.Application.UseCases.ContentPage.GetAllAllContentTemplates;
using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUpdateContentTemplateUseCase, UpdateContentTemplateUseCase>();
            services.AddScoped<IViewContentTemplateUseCase, ViewContentTemplateUseCase>();
            services.AddScoped<IGetEstablishmentDetailsUseCase, GetEstablishmentDetailsUseCase>();
            services.AddScoped<IEstablishmentSearchUseCase, EstablishmentSearchUseCase>();
            services.AddScoped<IEstablishmentSearchSuggestionsUseCase, EstablishmentSearchSuggestionsUseCase>();
            services.AddScoped<IGetAllContentTemplatesUseCase, GetAllContentTemplatesUseCase>();
            
            return services;
        }
    }
}
