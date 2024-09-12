using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthoritiesPageViewModel
{
    public string Title { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public LocalAuthoritiesPageViewModel(
        string title, 
        BreadcrumbTrailViewModel breadcrumbs
    )
    {
        Title = title;
        Breadcrumbs = breadcrumbs;
    }
}