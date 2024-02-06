using ASP.Core.PageContent.Repository;
using ErrorOr;
using ASP.Core.PageContent;
using ASP.Core;

namespace ASP.Infrastructure.Repositories
{
    public class PageContentRepository : IPageContentRepository
    {
        private const string ContainerKey = "content";
        private readonly IDocumentDatabase _documentDB;

        public PageContentRepository(IDocumentDatabase documentDB)
        {
            _documentDB = documentDB ??
                throw new ArgumentNullException(nameof(documentDB));
        }

        public Task<ErrorOr<PageContentTemplate>> Get(string id)
        {
            return _documentDB.GetAsync<PageContentTemplate>(ContainerKey, id, id);
        }

        public Task<ErrorOr<Updated>> Update(PageContentTemplate updatedPageContent)
        {
            return _documentDB.UpsertAsync(ContainerKey, updatedPageContent.id, updatedPageContent.contentId, updatedPageContent);
        }
    }
}
