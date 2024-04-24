using Microsoft.AspNetCore.Mvc.Razor;

namespace ASP.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApp(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddRouting(options => options.LowercaseUrls = true)
                .Configure<RazorViewEngineOptions>(options =>
                {
                    // {3} is the feature, {2} is the area, {1} is the controller, {0} is the action

                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Shared/{0}" + RazorViewEngine.ViewExtension);
                })
                .AddControllersWithViews();

            configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true);

            return services;
        }
    }
}