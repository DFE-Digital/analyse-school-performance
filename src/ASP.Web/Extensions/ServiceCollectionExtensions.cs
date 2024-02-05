using ASP.Application.UseCases.UpdateContentPage;
using ASP.Core.PageContent.Repository;
using ASP.Infrastructure.Repositories;
using ASP.Web.Services;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using DfE.Data.DynamicPageTemplates.Core;

namespace ASP.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection RegisterDFEComponentLibraries(this IServiceCollection services)
        {
            services.AddCosmosDbDependencies();
            services.AddDynamicPageCoreDependencies();

            return services;
        }

        internal static IServiceCollection RegisterWebServices(this IServiceCollection services)
        {
            services.AddScoped<INonceService>(serviceProvider => new NonceService(32));

            return services;
        }

        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPageContentRepository, PageContentRepository>();

            return services;
        }

       
    }
}
