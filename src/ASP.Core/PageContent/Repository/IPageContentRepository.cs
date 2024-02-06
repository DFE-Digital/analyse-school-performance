using ErrorOr;

namespace ASP.Core.PageContent.Repository
{
    public interface IPageContentRepository
    {
        public Task<ErrorOr<PageContentTemplate>> Get(string id);
        public Task<ErrorOr<Updated>> Update(PageContentTemplate updatedContentPage);
    }
}
