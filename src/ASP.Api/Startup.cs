using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASP.Infrastructure.Repositories;
using ASP.Infrastructure.Cosmos;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using ASP.Core.Templating;
using ASP.Core.Establishments;
using Azure.Identity;
using AppEnvironmentVariables = ASP.Infrastructure.Constants.EnvironmentVariables;
using ASP.Application.Extensions;


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
                    services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
                    services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
                    services.AddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();

                    services.AddCosmosDbDependencies();
                    services.AddUseCases();
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
                    
                    var builtConfig = builder.Build();

                    var keyVaultName = builtConfig[AppEnvironmentVariables.AspAzureKeyVaultName];

                    if (!string.IsNullOrEmpty(keyVaultName))
                    {
                        var credential = new DefaultAzureCredential();
                        builder.AddAzureKeyVault(new Uri($"https://{keyVaultName}.vault.azure.net/"), credential);
                    }
                });
        }
    }
}
