using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace ASP.Web.Core.Templating
{
    public static class TemplatingExtensions
    {
        public static IServiceCollection ConfigureComponentLibrary(this IServiceCollection services, Assembly componentAssembly)
        {
            services.AddMvcCore()
                .AddApplicationPart(componentAssembly)
                .AddControllersAsServices();

            services.Configure<TemplateComponentOptions>(options =>
            {
                options.ComponentAssemblies.Add(componentAssembly);
            });

            return services;
        }

        public static IServiceCollection RegisterTemplateComponentLocation(this IServiceCollection services, string componentPath)
        {
            services.Configure<TemplateComponentOptions>(options =>
            {
                options.ComponentLocations.Add(componentPath);
            });

            return services;
        }

        public static IServiceCollection ConfigureTemplateComponents(this IServiceCollection services)
        {
            services.AddSingleton<TemplateComponentEditModelTypeLocator>();
            
            services.AddSingleton<TemplateComponentEditModelBinderProvider>();
            services.AddTransient<IConfigureOptions<MvcOptions>, TemplateComponentsMvcOptionsSetup>();
            
            services.AddSingleton<ITemplateComponentEditModelFactory, TemplateComponentEditModelFactory>();
            services.AddTransient<IConfigureOptions<MvcViewOptions>, TemplateComponentsMvcViewOptionsSetup>();

            services.AddSingleton<TemplateComponentLocationExpander>();
            services.AddTransient<IConfigureOptions<RazorViewEngineOptions>, TemplateComponentsRazorViewEngineOptionsSetup>();

            return services;
        }

        public class TemplateComponentsMvcOptionsSetup : IConfigureOptions<MvcOptions>
        {
            private readonly TemplateComponentEditModelBinderProvider _editModelBinderProvider;

            public TemplateComponentsMvcOptionsSetup(TemplateComponentEditModelBinderProvider editModelBinderProvider)
            {
                _editModelBinderProvider = editModelBinderProvider
                    ?? throw new ArgumentNullException(nameof(editModelBinderProvider));
            }

            public void Configure(MvcOptions options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                options.ModelBinderProviders.Insert(0, _editModelBinderProvider);
            }
        }

        public class TemplateComponentsMvcViewOptionsSetup : IConfigureOptions<MvcViewOptions>
        {
            private readonly IOptions<TemplateComponentOptions> _options;

            public TemplateComponentsMvcViewOptionsSetup(IOptions<TemplateComponentOptions> options)
            {
                _options = options
                    ?? throw new ArgumentNullException(nameof(options));
            }

            public void Configure(MvcViewOptions options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                var existingViewEngine = options.ViewEngines.OfType<RazorViewEngine>().FirstOrDefault();
                if (existingViewEngine != null)
                {
                    options.ViewEngines.Remove(existingViewEngine);
                    options.ViewEngines.Add(new TemplateComponentsViewEngine(existingViewEngine, _options));
                }
            }
        }

        public class TemplateComponentsRazorViewEngineOptionsSetup : IConfigureOptions<RazorViewEngineOptions>
        {
            private readonly TemplateComponentLocationExpander _locationExpander;

            public TemplateComponentsRazorViewEngineOptionsSetup(TemplateComponentLocationExpander locationExpander)
            {
                _locationExpander = locationExpander
                    ?? throw new ArgumentNullException(nameof(locationExpander));
            }

            public void Configure(RazorViewEngineOptions options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                options.ViewLocationExpanders.Add(_locationExpander);
            }
        }
    }   
}
