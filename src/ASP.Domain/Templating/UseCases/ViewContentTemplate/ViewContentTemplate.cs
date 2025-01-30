using ASP.Core.Results;

namespace ASP.Domain.Templating.UseCases.ViewContentTemplate
{
    /// <summary>
    /// Views a Content Template or a Content Template revision, taking a ViewContentTemplateRequest object:
    ///     ContentTemplateId:   ID of the (base) Content Template to view
    ///     Revision (optional): Revision to view
    /// 
    /// A Content Template has a Base Template and a set of Revisions. The Base Template and Revisions are 
    /// the same document structure each having an ID and a ContentId. The Base Template is created first
    /// (in an unpublished state) with the ID the same as the ContentId. New Revisions can be created by copying
    /// an existing Revision with a new ID but the same ContentId (in an unpublished state). Any Revision can be
    /// published, and this sets any previously published Revision to be unpublished (there can only be one published
    /// Revision per Content Template).
    /// 
    /// Revision can be the same as ContentTemplateId in which case this is the Base Template.
    /// 
    /// If a Revision isn't provided:
    ///   If the Base Template with ID ContentTemplateId doesn't exist, returns a NotFound error
    ///   If the Base Template with ID ContentTemplateId exists and is unpublished, returns a NotFound error
    ///   If the Base Template with ID ContentTemplateId exists and is published, returns the Base Content Template object
    /// 
    /// If a Revision is provided:
    ///   If the Base Template with ID ContentTemplateId doesn't exist, returns a NotFound error
    ///   If the Revision doesn't exist, returns a NotFound error
    ///   If the Revision exists and is unpublished, returns the Revision Content Template object
    ///   If the Revision exists and is published, returns the Revision Content Template object
    /// </summary>
    public class ViewContentTemplate : IViewContentTemplate
    {
        private readonly IContentTemplateRepository _repository;

        public ViewContentTemplate(IContentTemplateRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public Task<Result<ContentTemplate>> HandleRequest(ViewContentTemplateRequest request)
        {
            return request.Revision.Match(
                revision =>
                    from _ in _repository.GetBaseTemplate(request.ContentTemplateId)
                    from template in _repository.GetRevision(request.ContentTemplateId, revision)
                    select template,
                () =>
                    from _ in _repository.GetBaseTemplate(request.ContentTemplateId)
                    from template in _repository.GetPublishedRevision(request.ContentTemplateId)
                    select template
            );
        }
    }
}
