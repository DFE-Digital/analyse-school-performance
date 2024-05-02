using ASP.Application.Extensions;
using ASP.Web.Features;
using ASP.Web.Areas;
using ASP.Web.Components;
using ASP.Web.Features.ContentSecurityPolicy;
using ASP.Web.Features.ErrorHandling;
using ASP.Web.Features.Logging;
using ASP.Web.Core.Templating;
using ASP.Web.Features.AnalyticsTrackingPreferences;
using ASP.Web.Features.ApplicationServiceVersion;
using ASP.Web.Features.ContentTemplates;
using ASP.Web.Features.Cookies;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Areas.School;

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
                .UseAppConfiguration(app.Environment)
                .UseContentSecurityPolicy(app.Environment)
                .UseLogging();
            app.MapControllers();
            app.Run();
        }
    }
}