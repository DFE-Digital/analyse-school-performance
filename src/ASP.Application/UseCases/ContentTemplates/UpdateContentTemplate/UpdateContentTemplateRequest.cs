using ASP.Core.Optionality;
using ASP.Core.Templating;

namespace ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate
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
