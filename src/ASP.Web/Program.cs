using ASP.Web.Extensions;
using ASP.Application.Extensions;
using ASP.Web.Models;
using ASP.Web.Filters;
using ASP.Web.ExceptionHandlers;
using ASP.Infrastructure.TableStorage;

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

            builder.Services.AddApplicationInsightsTelemetry();
            builder.Services.AddExceptionHandler<ExceptionHandlerServerError>();

            builder.Services.Configure<TableStorageConfiguration>(builder.Configuration.GetSection("TableStorage"));

            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile("appsettings.local.json", true);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
               // app.UseExceptionHandler("/home/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                //not used in dev
                app.UseNonce();
            }

            app.UseExceptionHandler("/home/error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}