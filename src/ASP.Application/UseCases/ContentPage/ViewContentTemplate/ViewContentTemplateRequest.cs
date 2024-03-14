namespace ASP.Application.UseCases.ViewContentTemplate
{
    public class ViewContentTemplateRequest
    {
        public string ContentTemplateId { get; set; }

        public ViewContentTemplateRequest(string contentTemplateId)
        {
            ContentTemplateId = contentTemplateId;
        }
    }
}
