using ASP.Application.UseCases.Downloads.DTO;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.DownloadData.SelectFiles;

public class DownloadDataSelectFilesModel : BaseDownloadDataModel
{
    public List<DownloadDto> AvailableDownloads { get; }
    public DownloadDataSelectFilesModel(BreadcrumbTrailViewModel breadcrumbs, NavigationViewModel? subNavigation,
        NavigationViewModel? sideNavigation, List<DownloadDto> availableDownloads, string title,
        string subTitle, string contentTitle, string contentTitleCaption, string controllerName, string controllerActionName) : base(breadcrumbs,
        subNavigation, sideNavigation, title, subTitle, contentTitle, contentTitleCaption, controllerName,
        controllerActionName)
    {
        AvailableDownloads = availableDownloads;
    }
}