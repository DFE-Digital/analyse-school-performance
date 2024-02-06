using MR;

namespace ASP.Core.PageContent
{
    public sealed class PageContentTemplate
    {
        public string id { get; set; } = null!;
        public string contentId { get; set; } = null!;
        public string? PageTitle { get; set; } = null!;
        public dynamic PageContent { get; set; } = new GracefulExpandoObject()!;
        public List<PageContentTemplateView> Views { get; set; } = new List<PageContentTemplateView>();
    }
}
