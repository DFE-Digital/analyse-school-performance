using ASP.Application.UseCases.UpdateContentTemplate;
using ASP.Application.UseCases.ViewContentTemplate;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUpdateContentTemplateUseCase, UpdateContentTemplateUseCase>();
            services.AddScoped<IViewContentTemplateUseCase, ViewContentTemplateUseCase>();

            return services;
        }
    }
}
