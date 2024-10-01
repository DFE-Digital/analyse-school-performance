using ASP.Infrastructure;
using ASP.Infrastructure.AzureKeyVault;
using ASP.Web.Areas;
using ASP.Web.Features;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ASP.Web
{
    public static class AspWebExtensions
    {
        public static IServiceCollection ConfigureRouting(this IServiceCollection services)
        {
            services
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.AppendTrailingSlash = true;
                });

            return services;
        }

        public static IServiceCollection ConfigureViews(this IServiceCollection services)
        {
            services
                .Configure<RazorViewEngineOptions>(options =>
                {
                    // {3} is the feature, {2} is the area, {1} is the controller, {0} is the action

                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Shared/{0}" + RazorViewEngine.ViewExtension);

                    options
                        .ConfigureFeatureViews()
                        .ConfigureAreaViews();
                })
                .AddControllersWithViews();

            return services;
        }

        /// <summary>
        /// This method configures various aspects of the application's environment, including key vaults and settings files.
        /// </summary>
        /// <param name="builder">Configuration builder object that allows configuration sources to be added</param>
        /// <param name="configuration">Configuration object representing state of configuration as it is being built.</param>
        /// <returns>The configuration builder</returns>
        public static IConfigurationBuilder ConfigureSettings(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            configuration.BindConfig<AzureKeyVaultOptions>(out var config);

            builder
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .AddEnvironmentVariables()
                // Adds a point for overriding/adding configuration sources that are then used to drive the
                // existing dependency injection. This allows test code to override configuration options without having
                // to rebuild the service collection. See also: ConfigurationInjection.cs
                // Based on workaround in https://github.com/dotnet/aspnetcore/issues/37680#issuecomment-1331559463
                .AddInjectedConfiguration()
                .ConfigureAzureKeyVault(config);

            return builder;
        }
    }
}