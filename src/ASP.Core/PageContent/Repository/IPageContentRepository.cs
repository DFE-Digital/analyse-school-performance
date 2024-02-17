using ErrorOr;

namespace ASP.Core.PageContent.Repository
{
    public interface IPageContentRepository
    {
        Task<ErrorOr<PageContentTemplate>> Get(string id);
        Task<ErrorOr<Updated>> Update(PageContentTemplate updatedContentPage);
        Task<ErrorOr<Deleted>> DeleteAll();
    }
}
