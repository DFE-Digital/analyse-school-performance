using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Application.UseCases.ViewContentTemplate;
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

            return services;
        }
    }
}
