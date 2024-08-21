using ASP.Web.Core.Templating;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Web.Components
{
    public static class ComponentsExtensions
    {
        public static IServiceCollection ConfigureWebComponents(this IServiceCollection services)
        {
            services
                .ConfigureComponentLibrary(typeof(ComponentsExtensions).Assembly)
                .RegisterTemplateComponentLocation("/Components");

            return services;
        }
    }
}
