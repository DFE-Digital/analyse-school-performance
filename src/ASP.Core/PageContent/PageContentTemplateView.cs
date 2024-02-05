using MR;

namespace ASP.Core.PageContent
{
    public sealed class PageContentTemplateView
    {
        public string id { get; set; } = default!;
        public string ViewId { get; set; } = null!;
        public dynamic ViewContent { get; set; } = new GracefulExpandoObject()!;
        public dynamic ViewModel { get; set; } = new GracefulExpandoObject()!;
        public List<PageContentTemplateView> ChildViews { get; set; } = new List<PageContentTemplateView>();
    }
}
