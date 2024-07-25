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
using ASP.Application;
using ASP.Infrastructure;

namespace ASP.Api
{
    public class Startup
    {
        public void Configure(IHostBuilder builder)
        {
            builder
                .ConfigureFunctionsWebApplication(builder =>
                    builder.UseMiddleware<ExceptionHandlingMiddleware>()
                )
                .ConfigureServices(services =>
                {
                    services.AddScoped<IContentTemplateRepository, ContentTemplateRepository>();
                    services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
                    services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
                    services.AddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();

                    services.AddCosmosDbDependencies();
                    services.AddUseCases();
                    services.ConfigureSearchStrategyFactory();
                    services.ConfigureSearchServices();

                    services.AddOptions<ErrorHandlingOptions>()
                       .Configure<IConfiguration>(
                           (settings, configuration) =>
                               configuration
                                   .GetSection(nameof(ErrorHandlingOptions))
                                   .Bind(settings));
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile(Path.Combine(
                            context.HostingEnvironment.ContentRootPath, "apisettings.json"),
                            optional: false)
                        .AddJsonFile(Path.Combine(
                            context.HostingEnvironment.ContentRootPath, "apisettings.local.json"),
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
