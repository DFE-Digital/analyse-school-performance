using ASP.Api.Client.ContentTemplates;
using ASP.Web.Core.BreadcrumbTrail;
using MR;

namespace ASP.Web.Core.Templating
{
    public class ContentTemplateViewModel
    {
        public string ContentId { get; set; } = "";
        public string? Revision { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public dynamic PageContent { get; set; } = new GracefulExpandoObject();
        public List<TemplateComponentViewModel> Views { get; set; } = [];
        public BreadcrumbTrailViewModel? Breadcrumbs { get; set; } = default!;

        public static ContentTemplateViewModel FromTemplate(string contentId, string? revision, ContentTemplate template)
        {
            return new ContentTemplateViewModel
            {
                ContentId = contentId,
                Revision = revision,
                PageTitle = template.PageTitle ?? "",
                PageContent = template.PageContent,
                Views = (template.Views ?? [])
                    .Select(TemplateComponentViewModel.FromTemplateView)
                    .ToList(),
                Breadcrumbs = new BreadcrumbTrailViewModel()
            };
        }
    }
}