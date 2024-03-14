using ASP.Core.Templating;

namespace ASP.Application.UseCases.UpdateContentTemplate
{
    public class UpdateContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }
        public ContentTemplate ContentTemplate { get; set; }

        public UpdateContentTemplateRequest(string contentTemplateId, ContentTemplate contentTemplate)
        {
            ContentTemplateId = contentTemplateId;
            ContentTemplate = contentTemplate;
        }
    }
}
