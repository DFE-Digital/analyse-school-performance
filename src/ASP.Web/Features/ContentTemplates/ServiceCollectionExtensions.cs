using ASP.Core;
using ASP.Infrastructure.Cosmos;
using ASP.Infrastructure.Repositories;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using ASP.Web.Core.Templating;
using ASP.Core.Templating;
using ASP.Core.Establishments;
using ASP.Infrastructure.Establishments;

namespace ASP.Web.Features.ContentTemplates
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureContentTemplates(this IServiceCollection services)
        {
            services.AddCosmosDbDependencies();
            services.AddScoped<IContentTemplateRepository, ContentTemplateRepository>();
            services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.AddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
            services.AddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();
            services.AddScoped<IRequestHostProvider, RequestHostProvider>();
            services.AddScoped<AttributeHelper>();
            services.AddScoped<MarkdownHelper>();
            services.AddHttpContextAccessor();

            services.RegisterTemplateComponentLocation("/Features/ContentTemplates/TemplateComponents");

            return services;
        }
    }
}
