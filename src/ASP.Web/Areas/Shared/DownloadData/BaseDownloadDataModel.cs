using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.DownloadData;

public class BaseDownloadDataModel
{
    public string Title { get; }
    public string SubTitle { get; }
    public string ContentTitle { get; }
    public string ContentTitleCaption { get; }
    public string ControllerName { get; }
    public string ControllerActionName { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }
    public NavigationViewModel? SubNavigation { get; }
    public NavigationViewModel? SideNavigation { get; }

    public BaseDownloadDataModel(BreadcrumbTrailViewModel breadcrumbs, NavigationViewModel? subNavigation,
        NavigationViewModel? sideNavigation, string title,
        string subTitle, string contentTitle, string contentTitleCaption, string controllerName, string controllerActionName)
    {
        Breadcrumbs = breadcrumbs;
        SubNavigation = subNavigation;
        SideNavigation = sideNavigation;
        Title = title;
        SubTitle = subTitle;
        ContentTitle = contentTitle;
        ControllerName = controllerName;
        ControllerActionName = controllerActionName;
        ContentTitleCaption = contentTitleCaption;
    }
}