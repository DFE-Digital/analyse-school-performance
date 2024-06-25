using ASP.Core.Results;

namespace ASP.Core.Templating
{
    public interface IContentTemplateRepository
    {
        Task<Result<ContentTemplate>> GetPublishedRevision(string contentTemplateId);
        Task<Result<ContentTemplate>> GetRevision(string contentTemplateId, string revision);
        Task<Result<ContentTemplate>> GetBaseTemplate(string contentTemplateId);
        Task<Result<Done>> Update(string contentTemplateId, string revision, ContentTemplate contentTemplate);
    }
}
