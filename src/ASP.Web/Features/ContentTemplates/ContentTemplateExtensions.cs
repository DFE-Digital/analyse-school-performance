using ASP.Core;
using ASP.Infrastructure.Cosmos;
using ASP.Infrastructure.Repositories;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using ASP.Web.Core.Templating;
using ASP.Core.Templating;
using ASP.Core.Establishments;
using ASP.Infrastructure.Establishments;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Web.Features.ContentTemplates
{
    public static class ContentTemplateExtensions
    {
        public static IServiceCollection ConfigureContentTemplates(this IServiceCollection services)
        {
            services.AddCosmosDbDependencies();
            services.TryAddScoped<IContentTemplateRepository, ContentTemplateRepository>();
            services.TryAddScoped<IEstablishmentRepository, EstablishmentRepository>();
            services.TryAddScoped<IDocumentDatabase, CosmosDocumentDatabase>();
            services.TryAddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();
            services.TryAddScoped<IRequestHostProvider, RequestHostProvider>();
            services.TryAddScoped<AttributeHelper>();
            services.TryAddScoped<MarkdownHelper>();
            services.AddHttpContextAccessor();

            services.RegisterTemplateComponentLocation("/Features/ContentTemplates/TemplateComponents");

            return services;
        }
    }
}
