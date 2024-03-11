using ASP.Core.PageContent;

namespace ASP.Web.Models
{
    public class ContentTemplateViewModel
    {
        public string ContentId { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public List<TemplateComponentViewModel> Views { get; set; } = new();

        public static ContentTemplateViewModel FromTemplate(string contentId, PageContentTemplate template)
        {
            return new ContentTemplateViewModel
            {
                ContentId = contentId,
                PageTitle = template.PageTitle ?? "",
                Views = (template.Views ?? new List<PageContentTemplateView>()).Select(TemplateComponentViewModel.FromTemplateView).ToList()
            };
        }
    }
}