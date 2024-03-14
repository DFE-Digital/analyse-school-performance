using ASP.Core.Templating.Repository;
using ErrorOr;
using ASP.Core.Templating;
using ASP.Core;

namespace ASP.Infrastructure.Repositories
{
    public class ContentTemplateRepository : IContentTemplateRepository
    {
        private const string ContainerKey = "content";
        private readonly IDocumentDatabase _documentDB;

        public ContentTemplateRepository(IDocumentDatabase documentDB)
        {
            _documentDB = documentDB ??
                throw new ArgumentNullException(nameof(documentDB));
        }

        public Task<ErrorOr<Deleted>> DeleteAll()
        {
            return _documentDB.DeleteAllAsync(ContainerKey);
        }

        public Task<ErrorOr<ContentTemplate>> Get(string id)
        {
            return _documentDB.GetAsync<ContentTemplateDTO>(ContainerKey, id, id)
                .Then(dto => dto.ToContentTemplate());
        }

        public Task<ErrorOr<Updated>> Update(string id, ContentTemplate contentTemplate)
        {
            var dto = new ContentTemplateDTO {
                id = id,
                contentId = id,
                PageTitle = contentTemplate.PageTitle,
                PageContent = contentTemplate.PageContent,
                Views = (contentTemplate.Views ?? new List<TemplateComponent>())
                    .Select(TemplateComponentDTO.FromTemplateComponent)
                    .ToList()
            };

            return _documentDB.UpsertAsync(ContainerKey, id, id, dto);
        }
    }
}
