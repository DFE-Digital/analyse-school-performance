using ErrorOr;

namespace ASP.Core.PageContent.Repository
{
    public interface IPageContentRepository
    {
        public Task<ErrorOr<Updated>> UpdatePageContent(PageContentTemplate updatedContentPage);
    }
}
