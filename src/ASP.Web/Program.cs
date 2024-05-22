using ASP.Application.Extensions;
using ASP.Web.Features;
using ASP.Web.Areas;
using ASP.Web.Components;
using ASP.Web.Areas.School;
using ASP.Web.Features.ContentSecurityPolicy;
using ASP.Web.Features.ErrorHandling;
using ASP.Web.Features.Logging;
using ASP.Web.Core.Templating;
using ASP.Web.Features.AnalyticsTrackingPreferences;
using ASP.Web.Features.ApplicationServiceVersion;
using ASP.Web.Features.ContentTemplates;
using ASP.Web.Features.Cookies;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Features.UrlRewriting;

namespace ASP.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services
                .ConfigureApp(builder.Configuration)
                .ConfigureAppEnvironment(builder.Configuration)
                .ConfigureAreas()
                .ConfigureFeatures(builder.Configuration)
                .ConfigureWebComponents()
                .AddUseCases()
                .ConfigureErrorHandling(builder.Configuration)
                .ConfigureContentTemplates()
                .ConfigureContentSecurityPolicy()
                .ConfigureApplicationServiceVersion()
                .ConfigureCookies()
                .ConfigureTermsOfUse()
                .ConfigureAnalyticsTrackingPreferences()
                .ConfigureTemplateComponents()
                .ConfigureLogging()
                .ConfigureSchoolPages();

            WebApplication app = builder.Build();

            app
                .UseErrorHandling(app.Environment)
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseAuthorization()
                .UseRouting()
                .UseContentSecurityPolicy(app.Environment)
                .UseLogging()
                .UseUrlRewriteRules();

            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.MapControllers();
            app.Run();
        }
    }
}