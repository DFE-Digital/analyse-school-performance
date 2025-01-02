using Microsoft.Extensions.Hosting;
using ASP.Application;
using ASP.Infrastructure;
using ASP.Infrastructure.Blob;
using Microsoft.Extensions.DependencyInjection;
using ASP.Infrastructure.DocumentDatabase;
using ASP.Core.Time;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace ASP.Api
{
    public class Startup
    {
        public void Configure(IHostBuilder builder)
        {
            builder
                .ConfigureOpenApi()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.ConfigureSettings(context.HostingEnvironment, context.Configuration);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .ConfigureDocumentDatabase(context.Configuration)
                        .ConfigureBlobStorage(context.Configuration)
                        .RegisterUseCases()
                        .RegisterRepositories()
                        .ConfigureDataDownloads(context.Configuration)
                        .AddScoped<ApiResultConverter>()
                        .ConfigureCurrentTime()
                        .AddOpenApiConfiguration();
                })
                .ConfigureFunctionsWebApplication(app =>
                    app.UseErrorHandling()
                );
        }
    }
}
