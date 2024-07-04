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

        public Task<Result<ContentTemplate>> GetPublishedRevision(string contentTemplateId)
        {
            return _documentDB.QueryAsync<ContentTemplateDTO>(ContainerKey, q => q.Where(t => t.ContentId == contentTemplateId && t.IsPublished))
                .ErrorIf(dtos => dtos.Count() == 0, Error.NotFound($@"Could not find a published revision for content template ""{contentTemplateId}""."))
                .Map(dtos => dtos.First().ToContentTemplate());
        }

        public Task<Result<ContentTemplate>> GetRevision(string contentTemplateId, string revision)
        {
            return _documentDB.QueryAsync<ContentTemplateDTO>(ContainerKey, q => q.Where(t => t.Id == revision && t.ContentId == contentTemplateId))
                .ErrorIf(dtos => dtos.Count() == 0, Error.NotFound($@"Could not find revision ""{revision}"" for content template ""{contentTemplateId}""."))
                .Map(dtos => dtos.First().ToContentTemplate());
        }

        public Task<Result<ContentTemplate>> GetBaseTemplate(string contentTemplateId)
        {
            return _documentDB.GetAsync<ContentTemplateDTO>(ContainerKey, contentTemplateId, contentTemplateId)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"Could not find content template ""{contentTemplateId}"".")
                    : e)
                .Map(dto => dto.ToContentTemplate());
        }

        public async Task<Result<Done>> Update(string contentTemplateId, string revision, ContentTemplate contentTemplate)
        {
            return await _documentDB.UpsertAsync(ContainerKey, revision, contentTemplateId, new ContentTemplateDTO {
                Id = revision,
                ContentId = contentTemplateId,
                PageTitle = contentTemplate.PageTitle,
                PageContent = contentTemplate.PageContent,
                Views = (contentTemplate.Views ?? new List<TemplateComponent>())
                    .Select(TemplateComponentDTO.FromTemplateComponent)
                    .ToList()
            });
        }

        public async Task<Result<List<ContentTemplate>>> GetAllPublishedTemplates()
        {
            return await _documentDB.QueryAsync<ContentTemplateDTO>(ContainerKey, q => q.Where(t => t.IsPublished))
                .ErrorIf(dtos => !dtos.Any(), Error.NotFound("Could not find any published revision content templates"))
                .Map(dtos => dtos.Select(dto => dto.ToContentTemplate()).ToList());
        }
    }
}
