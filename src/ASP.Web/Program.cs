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
                .ConfigureErrorHandling(builder.Configuration)
                .ConfigureContentTemplates()
                .ConfigureContentSecurityPolicy()
                .ConfigureApplicationServiceVersion()
                .ConfigureCookies()
                .ConfigureTermsOfUse()
                .ConfigureAnalyticsTrackingPreferences()
                .ConfigureTemplateComponents()
                .ConfigureLogging()
                .ConfigureSchoolPages()
                .ConfigureSearch();

            builder.Services.AddUseCases();
            builder.Services.AddScoped<IAspApi, UseCaseReferenceApi>();

            // TODO: Replace API with HTTP implementation when fully working!
            //builder.Services.AddScoped<IAspApi, HttpApi>();

            //builder.Services.AddOptions<ApiOptions>()
            //   .Configure<IConfiguration>(
            //       (settings, configuration) =>
            //           configuration
            //               .GetSection(nameof(ApiOptions))
            //               .Bind(settings));

            WebApplication app = builder.Build();

            // this must be called before the UseExceptionHandler so that it can pass the correct errors message through to the view
            // see CustomPageNotFoundMiddleware comments for how it works
            app.UseMiddleware<StatusCodePageLoggingMiddleware>();

            // we only want to call exception handling middleware during production or testing the production server error pages
            if (EnvironmentHelper.ShouldUseProductionErrorPage(app.Configuration, app.Environment))
            {
                app.UseExceptionHandler("/error/");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            if (app.Environment.IsProduction())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseUrlRewriteRules();
            app.UseRouting();
            app.UseAuthorization();
            app.UseContentSecurityPolicy(app.Environment);

            app.MapControllers();
            app.Run();
        }
    }
}