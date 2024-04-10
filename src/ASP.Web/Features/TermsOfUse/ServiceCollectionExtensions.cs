using ASP.Web.Core.Templating;

namespace ASP.Web.Features.TermsOfUse
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureTermsOfUse(this IServiceCollection services)
        {
            services.AddScoped<TermsOfUseActionFilter>();

            services.RegisterTemplateComponentLocation("/Features/TermsOfUse/TemplateComponents");

            return services;
        }
    }
}
