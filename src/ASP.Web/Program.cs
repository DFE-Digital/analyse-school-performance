using ASP.Web.Extensions;
using ASP.Application.Extensions;
using ASP.Web.Models;
using ASP.Web.Filters;
using ASP.Infrastructure.TableStorage;
using ASP.Web.ExceptionHandlers;

namespace ASP.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddControllersWithViews(options =>  
                {
                    options.ModelBinderProviders.Insert(0, new TemplateComponentEditModelBinderProvider());
                    options.Filters.Add(typeof(CheckCookies));
                    options.Filters.Add(typeof(CurrentVersionActionFilter));
                });
            builder.Services.RegisterDFEComponentLibraries()
                .RegisterWebServices()
                .RegisterRepositories()
                .RegisterUseCases();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddExceptionHandler<ExceptionHandlerServerError>();

            builder.Services.Configure<TableStorageConfiguration>(builder.Configuration.GetSection("TableStorage"));

            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile("appsettings.local.json", true);

            WebApplication app = builder.Build();

            app.UsePageNotFoundLogging();
            app.UseCustomPageNotFound(app.Environment);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                //not used in dev
                app.UseNonce();
                
                // Handle status code exceptions like page not found
                app.UseStatusCodePagesWithReExecute("/home/error", "?statusCode={0}");
            }
            
            app.UseExceptionHandler("/home/error"); 
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}