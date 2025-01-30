using ASP.Core.Optionality;

namespace ASP.Domain.Templating.UseCases.UpdateContentTemplate
{
    public class UpdateContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }
        public Optional<string> Revision { get; set; }
        public ContentTemplate ContentTemplate { get; set; }

        public UpdateContentTemplateRequest(string contentTemplateId, Optional<string> revision, ContentTemplate contentTemplate)
        {
            ContentTemplateId = contentTemplateId;
            Revision = revision;
            ContentTemplate = contentTemplate;
        }
    }
}
