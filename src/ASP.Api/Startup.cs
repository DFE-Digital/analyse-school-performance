using ASP.Core.Templating.Repository;
using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASP.Infrastructure.Repositories;
using ASP.Infrastructure.Cosmos;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Application.UseCases.UpdateContentTemplate;

namespace ASP.Api
{
    public class Startup
    {
        public void Configure(IHostBuilder builder)
        {
            builder
                .ConfigureFunctionsWebApplication()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IContentTemplateRepository, ContentTemplateRepository>();
                    services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
                    services.AddScoped<ITableStorageProvider, CosmosDbQueryHandler>();

                    services.AddCosmosDbDependencies();

                    services.AddScoped<IViewContentTemplateUseCase, ViewContentTemplateUseCase>();
                    services.AddScoped<IUpdateContentTemplateUseCase, UpdateContentTemplateUseCase>();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile(Path.Combine(
                            context.HostingEnvironment.ContentRootPath, "appsettings.json"),
                            optional: false)
                        .AddJsonFile(Path.Combine(
                            context.HostingEnvironment.ContentRootPath, "appsettings.local.json"),
                            optional: true)
                        .AddEnvironmentVariables();
                });
        }
    }
}
