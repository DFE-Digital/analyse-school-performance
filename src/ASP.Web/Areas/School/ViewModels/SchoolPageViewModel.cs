using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolPageViewModel
{
    public string Controller { get; set; }
    public string Title { get; }
    public string SubTitle { get; }
    public string SchoolName { get; set; }
    public string SchoolUrn { get; set; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }
    public NavigationViewModel? SubNavigation { get; set; }
    public NavigationViewModel? SideNavigation { get; set; }

    public SchoolPageViewModel(
        string controller,
        string title,
        string subTitle,
        string schoolName,
        string schoolUrn,
        BreadcrumbTrailViewModel breadcrumbs,
        NavigationViewModel? subNavigation,
        NavigationViewModel? sideNavigation
    )
    {
        Controller = controller;
        Title = title;
        SubTitle = subTitle;
        SchoolName = schoolName;
        SchoolUrn = schoolUrn;
        Breadcrumbs = breadcrumbs;
        SubNavigation = subNavigation;
        SideNavigation = sideNavigation;
    }
}