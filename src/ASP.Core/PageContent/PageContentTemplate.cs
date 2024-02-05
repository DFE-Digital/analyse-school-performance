using DfE.Data.DynamicPageTemplates.Web.DynamicPages.ViewModels;
using MR;

namespace ASP.Core.PageContent
{
    public sealed class PageContentTemplate
    {
        public string id { get; set; } = null!;
        public string contentId { get; set; } = null!;
        public string? PageTitle { get; set; } = null!;
        public dynamic PageContent { get; set; } = new GracefulExpandoObject()!;
        public List<DynamicViewModel> Views { get; set; } = new List<DynamicViewModel>();
        public string? EditableJson { get; set; }

        public PageContentTemplate()
        {

        }

        public static PageContentTemplate CreatePageContentTemplate(string id, string pageTitle, dynamic pageContent, List<DynamicViewModel> views, string? json = null)
        {
            return new PageContentTemplate()
            {
                id = id,
                PageTitle = pageTitle,
                PageContent = pageContent,
                Views = views,
                EditableJson = json
            };
        }
    }
}
