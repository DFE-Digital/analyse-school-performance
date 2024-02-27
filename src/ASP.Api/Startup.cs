using ASP.Core.PageContent.Repository;
using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASP.Infrastructure.Repositories;
using ASP.Infrastructure.Cosmos;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using ASP.Application.UseCases.ViewContentPage;
using ASP.Application.UseCases.UpdateContentPage;

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
                    services.AddScoped<IPageContentRepository, PageContentRepository>();
                    services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
                    services.AddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();

                    services.AddCosmosDbDependencies();

                    services.AddScoped<IViewContentPageUseCase, ViewContentPageUseCase>();
                    services.AddScoped<IUpdateContentPageUseCase, UpdateContentPageUseCase>();
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
