using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolsPageViewModel
{
    public string Title { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public SchoolsPageViewModel(
        string title, 
        BreadcrumbTrailViewModel breadcrumbs
    )
    {
        Title = title;
        Breadcrumbs = breadcrumbs;
    }
}