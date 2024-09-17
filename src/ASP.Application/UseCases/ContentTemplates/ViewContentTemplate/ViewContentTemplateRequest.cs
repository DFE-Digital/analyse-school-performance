using ASP.Core.Optionality;

namespace ASP.Application.UseCases.ContentTemplates.ViewContentTemplate
{
    public class ViewContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }
        public Optional<string> Revision { get; set; }

        public ViewContentTemplateRequest(string contentTemplateId, Optional<string> revision)
        {
            ContentTemplateId = contentTemplateId;
            Revision = revision;
        }
    }
}
