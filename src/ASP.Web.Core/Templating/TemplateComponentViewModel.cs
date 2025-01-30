using ASP.Domain.Templating;
using MR;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentViewModel
    {
        public string ViewId { get; set; } = "";
        public dynamic ViewContent { get; set; } = new GracefulExpandoObject();
        public List<TemplateComponentViewModel> ChildViews { get; set; } = new();

        public static TemplateComponentViewModel FromTemplateView(TemplateComponent v)
        {
            return new TemplateComponentViewModel
            {
                ViewId = v.ViewId,
                ViewContent = v.ViewContent ?? new GracefulExpandoObject(),
                ChildViews = (v.ChildViews ?? new List<TemplateComponent>()).Select(FromTemplateView).ToList()
            };
        }
    }
}
