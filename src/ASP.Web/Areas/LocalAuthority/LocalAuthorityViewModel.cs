using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityViewModel
{
    public string Name { get; }
    public ContentTemplateViewModel ContentTemplate { get; }
    public BreadcrumbViewModel Breadcrumbs { get; }

    public LocalAuthorityViewModel(string name, ContentTemplateViewModel contentTemplate, BreadcrumbViewModel breadcrumbs)
    {
        Name = name;
        ContentTemplate = contentTemplate;
        Breadcrumbs = breadcrumbs;
    }
}