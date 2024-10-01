using Microsoft.Extensions.Hosting;
using ASP.Application;
using ASP.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Api
{
    public class Startup
    {
        public void Configure(IHostBuilder builder)
        {
            builder
                .ConfigureAppConfiguration((context, builder) => 
                {
                    builder.ConfigureSettings(context.HostingEnvironment, context.Configuration);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .ConfigureDocumentDatabase(context.Configuration)
                        .RegisterUseCases()
                        .RegisterRepositories()
                        .AddScoped<ApiResultConverter>();
                })
                .ConfigureFunctionsWebApplication(app =>
                    app.UseErrorHandling()
                );
        }
    }
}
