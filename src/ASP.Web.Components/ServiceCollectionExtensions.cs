using ASP.Web.Core.Templating;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Web.Components
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureWebComponents(this IServiceCollection services)
        {
            services
                .ConfigureComponentLibrary(typeof(ServiceCollectionExtensions).Assembly)
                .RegisterTemplateComponentLocation("/Components");

            return services;
        }
    }
}
