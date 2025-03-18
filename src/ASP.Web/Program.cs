using ASP.Api.Client.InProcess;
using ASP.Core.Time;
using ASP.Infrastructure.Azure.Blob;
using ASP.Infrastructure.Azure.CosmosDb;
using ASP.Infrastructure.Azure.TableStorage;
using ASP.Infrastructure.Logging;
using ASP.Web.Components;
using ASP.Web.Core.Templating;
using ASP.Web.Extensions;
using ASP.Web.Features;
using ASP.Web.Features.AnalyticsTrackingPreferences;
using ASP.Web.Features.ApplicationServiceVersion;
using ASP.Web.Features.Authentication;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.ContentSecurityPolicy;
using ASP.Web.Features.ContentTemplates;
using ASP.Web.Features.Cookies;
using ASP.Web.Features.ErrorHandling;
using ASP.Web.Features.Search;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Features.UrlRewriting;

namespace ASP.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .ConfigureSettings(builder.Environment, builder.Configuration);

        builder.Services
            .ConfigureRouting()
            .ConfigureFeatures()
            .ConfigureViews()
            .ConfigureWebComponents()
            .ConfigureAuthentication(builder.Configuration)
            .ConfigureAuthorization(builder.Configuration)
            .ConfigureErrorHandling(builder.Configuration, out var errorHandlingConfig)
            .ConfigureBlobStorage(builder.Configuration)
            .ConfigureDocumentDatabase(builder.Configuration)
            .ConfigureTableStorage(builder.Configuration)
            .ConfigureApiClient(builder.Configuration)
            .ConfigureContentTemplates()
            .ConfigureContentSecurityPolicy()
            .ConfigureApplicationServiceVersion()
            .ConfigureCookies()
            .ConfigureTermsOfUse()
            .ConfigureAnalyticsTrackingPreferences()
            .ConfigureTemplateComponents()
            .ConfigureLogging()
            .ConfigureCurrentTime()
            .ConfigureSearch(builder.Configuration);

        WebApplication app = builder.Build();

        app.UseErrorHandling(app.Environment, errorHandlingConfig);

        if (app.Environment.IsProduction())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection()
           .UseStaticFiles()
           .UseUrlRewriteRules()
           .UseRouting()
           .UseCookieContentSecurityPolicy()
           .UseAuthentication()
           .UseAuthorization()
           .UseContentSecurityPolicy(app.Environment);

        app.MapControllers();
        app.Run();
    }
}
