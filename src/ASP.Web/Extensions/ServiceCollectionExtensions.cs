using ASP.Core;
using ASP.Core.Establishments.Repository;
using ASP.Core.Templating.Repository;
using ASP.Infrastructure.Cosmos;
using ASP.Infrastructure.Repositories;
using ASP.Infrastructure.TableStorage;
using ASP.Web.Filters;
using ASP.Web.Helpers;
using ASP.Web.Services;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;

namespace ASP.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterDFEComponentLibraries(this IServiceCollection services)
        {
            services.AddCosmosDbDependencies();

            return services;
        }

        internal static IServiceCollection RegisterWebServices(this IServiceCollection services)
        {
            services.AddScoped<INonceService>(serviceProvider => new NonceService(32));
            services.AddScoped<TermsOfUseActionFilter>();
            
            // providers
            services.AddSingleton<ICurrentVersionProvider, GitCommitHashCurrentVersionProvider>();
            services.AddScoped<ICookieProvider, CookieProvider>();
            services.AddScoped<IRequestHostProvider, RequestHostProvider>();

            services.AddScoped<AttributeHelper>();
            services.AddScoped<MarkdownHelper>();

            return services;
        }

        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IContentTemplateRepository, ContentTemplateRepository>();
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
            services.AddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();
            services.AddSingleton<ITableStorageProvider, TableStorageProvider>();

            return services;
        }


    }
}
