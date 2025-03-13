using ASP.Core.Time;
using ASP.Domain;
using ASP.Domain.Repositories;
using ASP.Infrastructure.Azure.Blob;
using ASP.Infrastructure.Azure.CosmosDb;
using ASP.Infrastructure.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                        .ConfigureContentTemplates()
                        .ConfigureSchools()
                        .ConfigureLocalAuthorities()
                        .ConfigureMultiAcademyTrusts()
                        .ConfigureDataDownloads(context.Configuration)
                        .RegisterRepositories()
                        .AddScoped<ApiResultConverter>()
                        .ConfigureLogging()
                        .ConfigureCurrentTime()
                        .AddOpenApiConfiguration();
                })
                .ConfigureFunctionsWebApplication(app =>
                    app.UseErrorHandling()
                );
        }
    }
}
