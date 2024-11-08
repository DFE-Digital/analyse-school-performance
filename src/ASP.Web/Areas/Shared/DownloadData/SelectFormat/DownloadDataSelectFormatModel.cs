using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.DownloadData.SelectFormat;

public class DownloadDataSelectFormatModel : BaseDownloadDataModel
{
    public string LinkText { get; }
    public string FileType { get; }
    public List<string> SelectedFiles { get; }
    public string SchoolOrLaLevel { get; }

    public DownloadDataSelectFormatModel(BreadcrumbTrailViewModel breadcrumbs, NavigationViewModel? subNavigation,
        NavigationViewModel? sideNavigation, string title,
        string subTitle, string contentTitle, string contentTitleCaption, string controllerName, string controllerActionName, string linkText,
        string fileType, List<string> selectedFiles, string schoolOrLaLevel) : base(breadcrumbs,
        subNavigation, sideNavigation, title, subTitle, contentTitle, contentTitleCaption, controllerName,
        controllerActionName)
    {
        LinkText = linkText;
        FileType = fileType;
        SelectedFiles = selectedFiles;
        SchoolOrLaLevel = schoolOrLaLevel;
    }
}