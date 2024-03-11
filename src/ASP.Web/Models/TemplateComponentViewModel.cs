using ASP.Core.PageContent;
using MR;

namespace ASP.Web.Models
{
    public class TemplateComponentViewModel
    {
        public string ViewId { get; set; } = "";
        public dynamic ViewContent { get; set; } = new GracefulExpandoObject();
        public List<TemplateComponentViewModel> ChildViews { get; set; } = new();

        public static TemplateComponentViewModel FromTemplateView(PageContentTemplateView v)
        {
            return new TemplateComponentViewModel
            {
                ViewId = v.ViewId,
                ViewContent = v.ViewContent ?? new GracefulExpandoObject(),
                ChildViews = (v.ChildViews ?? new List<PageContentTemplateView>()).Select(FromTemplateView).ToList()
            };
        }
    }
}
