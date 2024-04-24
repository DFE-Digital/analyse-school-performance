using ASP.Core.Templating;
using ASP.Core;
using ASP.Core.Results;

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

        public Task<Result<Done>> DeleteAll()
        {
            return _documentDB.DeleteAllAsync(ContainerKey);
        }

        public Task<Result<ContentTemplate>> Get(string contentId)
        {
            return _documentDB.GetAsync<ContentTemplateDTO>(ContainerKey, contentId, contentId)
                .Map(dto => dto.ToContentTemplate());
        }

        public Task<Result<Done>> Update(string contentId, ContentTemplate contentTemplate)
        {
            var dto = new ContentTemplateDTO {
                id = contentId,
                contentId = contentId,
                PageTitle = contentTemplate.PageTitle,
                PageContent = contentTemplate.PageContent,
                Views = (contentTemplate.Views ?? new List<TemplateComponent>())
                    .Select(TemplateComponentDTO.FromTemplateComponent)
                    .ToList()
            };

            return _documentDB.UpsertAsync(ContainerKey, contentId, contentId, dto);
        }
    }
}
