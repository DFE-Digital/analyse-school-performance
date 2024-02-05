using ASP.Application.UseCases.UpdateContentPage;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterUseCases(this IServiceCollection services)
        {
            services.AddScoped<IUpdateContentPageUseCase, UpdateContentPageUseCase>();

            return services;
        }
    }
}
