using ASP.Web.Core.Templating;

namespace ASP.Web.Features.TermsOfUse
{
    public static class TermsOfUseExtensions
    {
        public static IServiceCollection ConfigureTermsOfUse(this IServiceCollection services)
        {
            services.AddScoped<TermsOfUseActionFilter>();

            services.RegisterTemplateComponentLocation("/Features/TermsOfUse/TemplateComponents");

            return services;
        }
    }
}
