using ASP.Web.Core.Templating;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Web.Features.ContentTemplates
{
    public static class ContentTemplateExtensions
    {
        public static IServiceCollection ConfigureContentTemplates(this IServiceCollection services)
        {
            services.TryAddScoped<IRequestHostProvider, RequestHostProvider>();
            services.TryAddScoped<AttributeHelper>();
            services.TryAddScoped<MarkdownHelper>();
            services.AddHttpContextAccessor();

            services.RegisterTemplateComponentLocation("/Features/ContentTemplates/TemplateComponents");

            return services;
        }
    }
}
