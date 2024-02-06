using ASP.Application.UseCases.UpdateContentPage;
using ASP.Application.UseCases.ViewContentPage;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUpdateContentPageUseCase, UpdateContentPageUseCase>();
            services.AddScoped<IViewContentPageUseCase, ViewContentPageUseCase>();

            return services;
        }
    }
}
