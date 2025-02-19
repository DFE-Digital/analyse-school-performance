using ASP.Core.Optionality;

namespace ASP.Domain.Templating.UseCases.GetContentTemplate
{
    public class GetContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }
        public Optional<string> Revision { get; set; }

        public GetContentTemplateRequest(string contentTemplateId, Optional<string> revision)
        {
            ContentTemplateId = contentTemplateId;
            Revision = revision;
        }
    }
}
