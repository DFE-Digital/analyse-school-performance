using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Features.DataDownloads
{
    public record DownloadDataStepViewModel(string StepTitle, BreadcrumbTrailViewModel BreadcrumbTrail, DownloadDataViewModel DownloadData);
}