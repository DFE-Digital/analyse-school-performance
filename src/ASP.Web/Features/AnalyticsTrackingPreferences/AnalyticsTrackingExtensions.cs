using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.AnalyticsTrackingPreferences
{
    public static class AnalyticsTrackingExtensions
    {
        public static IServiceCollection ConfigureAnalyticsTrackingPreferences(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(AnalyticsTrackingActionFilter));
            });

            services.Configure<TemplateComponentOptions>(options =>
            {
                options.ComponentLocations.Add("/Features/AnalyticsTrackingPreferences/TemplateComponents");
            });

            return services;
        }
    }
}
