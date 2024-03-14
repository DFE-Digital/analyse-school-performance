using ErrorOr;

namespace ASP.Core.Templating.Repository
{
    public interface IContentTemplateRepository
    {
        Task<ErrorOr<ContentTemplate>> Get(string id);
        Task<ErrorOr<Updated>> Update(string id, ContentTemplate contentTemplate);
        Task<ErrorOr<Deleted>> DeleteAll();
    }
}
