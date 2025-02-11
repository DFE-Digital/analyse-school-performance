using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Features.DataDownloads
{
    public record DownloadDataSubActionViewModel(string StepTitle, BreadcrumbTrailViewModel BreadcrumbTrail, DownloadDataViewModel DownloadData);
}