using ASP.Application.UseCases.Downloads;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.DownloadData.SelectYear;

public class DownloadDataSelectYearModel : BaseDownloadDataModel
{
    public List<AcademicYear> AvailableDates { get; }
    public DownloadDataSelectYearModel(BreadcrumbTrailViewModel breadcrumbs, NavigationViewModel? subNavigation,
        NavigationViewModel? sideNavigation, List<AcademicYear> availableDates, string title,
        string subTitle, string contentTitle, string contentTitleCaption, string controllerName, string controllerActionName) : base(breadcrumbs,
        subNavigation, sideNavigation, title, subTitle, contentTitle, contentTitleCaption, controllerName,
        controllerActionName)
    {
        AvailableDates = availableDates;
    }
}