using ASP.Core.Templating;
using MR;

namespace ASP.Web.Core.Templating
{
    public class ContentTemplateViewModel
    {
        public string ContentId { get; set; } = "";
        public string? Revision { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public dynamic PageContent { get; set; } = new GracefulExpandoObject();
        public List<TemplateComponentViewModel> Views { get; set; } = new();

        public static ContentTemplateViewModel FromTemplate(string contentId, string? revision, ContentTemplate template)
        {
            return new ContentTemplateViewModel
            {
                ContentId = contentId,
                Revision = revision,
                PageTitle = template.PageTitle ?? "",
                PageContent = template.PageContent,
                Views = (template.Views ?? new List<TemplateComponent>()).Select(TemplateComponentViewModel.FromTemplateView).ToList()
            };
        }
    }
}