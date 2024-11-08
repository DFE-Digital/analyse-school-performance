using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityPageViewModel
{
    public string Title { get; }
    public string Name { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public LocalAuthorityPageViewModel(
        string title, 
        string name, 
        BreadcrumbTrailViewModel breadcrumbs
    )
    {
        Title = title;
        Name = name;
        Breadcrumbs = breadcrumbs;
    }
}