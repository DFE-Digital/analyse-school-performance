using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityViewModel
{
    public string Title { get; }
    public string Name { get; }
    public ContentTemplateViewModel ContentTemplate { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public LocalAuthorityViewModel(string title, string name, ContentTemplateViewModel contentTemplate, BreadcrumbTrailViewModel breadcrumbs)
    {
        Title = title;
        Name = name;
        ContentTemplate = contentTemplate;
        Breadcrumbs = breadcrumbs;
    }
}