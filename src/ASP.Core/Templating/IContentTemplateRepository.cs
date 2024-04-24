using ASP.Core.Results;

namespace ASP.Core.Templating
{
    public interface IContentTemplateRepository
    {
        Task<Result<ContentTemplate>> Get(string contentId);
        Task<Result<Done>> Update(string contentId, ContentTemplate contentTemplate);
        Task<Result<Done>> DeleteAll();
    }
}
