using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ASP.Web.Features
{
    public static class FeaturesExtensions
    {
        public static IServiceCollection ConfigureFeatures(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new FeatureConvention());
            });

            return services;
        }

        public static RazorViewEngineOptions ConfigureFeatureViews(this RazorViewEngineOptions options)
        {
            // {3} is the feature, {2} is the area, {1} is the controller,{0} is the action

            options.ViewLocationFormats.Add("/Features/{3}/{0}" + RazorViewEngine.ViewExtension);
            options.ViewLocationFormats.Add("/Features/{3}/Views/{0}" + RazorViewEngine.ViewExtension);
            options.ViewLocationFormats.Add("/Features/Shared/{0}" + RazorViewEngine.ViewExtension);

            var expander = new FeatureViewLocationExpander();
            options.ViewLocationExpanders.Add(expander);

            return options;
        }
    }
}
