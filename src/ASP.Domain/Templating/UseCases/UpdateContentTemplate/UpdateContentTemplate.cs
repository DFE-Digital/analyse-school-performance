using ASP.Core.Results;

namespace ASP.Domain.Templating.UseCases.UpdateContentTemplate
{
    /// <summary>
    /// Updates or creates a Content Template or a Content Template revision, taking a UpdateContentTemplateRequest object:
    ///     ContentTemplateId:   ID of the (base) Content Template to update or create
    ///     Revision (optional): Revision to update or create
    ///     ContentTemplate:     Content Template object to insert or replace instead of an existing one
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
    ///   If the Base Template with ID ContentTemplateId doesn't exist, creates the unpublished Base Template
    ///   If the Base Template with ID ContentTemplateId exists and is unpublished, updates it
    ///   If the Base Template with ID ContentTemplateId exists and is published, returns a NotAllowed error (published 
    ///       Revisions can't be updated)
    /// 
    /// If a Revision is provided:
    ///   If the Base Template with ID ContentTemplateId doesn't exist, returns a NotFound error
    ///   If the Revision doesn't exist, creates the unpublished Revision
    ///   If the Revision exists and is unpublished, updates it
    ///   If the Revision exists and is published, returns a NotAllowed error (published 
    ///       Revisions can't be updated)
    /// </summary>
    public class UpdateContentTemplate : IUpdateContentTemplate
    {
        private readonly IContentTemplateRepository _repository;

        public UpdateContentTemplate(IContentTemplateRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public Task<Result<Done>> HandleRequest(UpdateContentTemplateRequest request)
        {
            var notAllowedError = Error.NotAllowed("Only unpublished Content Template revisions can be updated.");

            return request.Revision.Match(
                revision =>
                    from _ in _repository.GetBaseTemplate(request.ContentTemplateId)
                    from t in _repository.GetRevision(request.ContentTemplateId, revision)
                        .ErrorIf(t => t.IsPublished, notAllowedError)
                        .DefaultIf(e => e is NotFoundError, request.ContentTemplate)
                    from done in _repository.Update(request.ContentTemplateId, revision, request.ContentTemplate)
                    select done,
                () =>
                    from t in _repository.GetBaseTemplate(request.ContentTemplateId)
                        .ErrorIf(t => t.IsPublished, notAllowedError)
                        .DefaultIf(e => e is NotFoundError, request.ContentTemplate)
                    from done in _repository.Update(request.ContentTemplateId, request.ContentTemplateId, request.ContentTemplate)
                    select done
            );
        }
    }
}