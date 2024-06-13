namespace ASP.Application.UseCases.ContentPage.ViewContentTemplate
{
    public class ViewContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }
        public string? Revision { get; set; }

        public ViewContentTemplateRequest(string contentTemplateId, string? revision)
        {
            ContentTemplateId = contentTemplateId;
            Revision = revision;
        }
    }
}
