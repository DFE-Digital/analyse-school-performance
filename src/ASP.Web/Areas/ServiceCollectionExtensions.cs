using Microsoft.AspNetCore.Mvc.Razor;

namespace ASP.Web.Areas
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAreas(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // {3} is the feature, {2} is the area, {1} is the controller,{0} is the action

                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Areas/Shared/{0}" + RazorViewEngine.ViewExtension);
                options.AreaViewLocationFormats.Add("/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            return services;
        }
    }
}
