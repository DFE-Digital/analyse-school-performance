using ASP.Application;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableLADownloads;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.Shared.DownloadData;
using ASP.Web.Areas.Shared.DownloadData.SelectFiles;
using ASP.Web.Areas.Shared.DownloadData.SelectFormat;
using ASP.Web.Areas.Shared.DownloadData.SelectYear;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    public abstract class LocalAuthorityController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected LocalAuthorityController(IAspApiClient api, IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected Task<Result<LocalAuthorityContentPageViewModel>> LandingPage(string laCode, string? revision)
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                from page in GetLocalAuthorityPage(laCode, laName, GetLandingPageBreadcrumbs(laCode, laName))
                select new LocalAuthorityContentPageViewModel(
                    page,
                    contentTemplate
                );
        }

        protected Task<Result<DownloadDataSelectYearModel>> DownloadDataSelectYear(string laCode, Func<string,
            BreadcrumbTrailViewModel> buildBreadcrumbs, string actionName)
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, Optional<int>.None)
                let breadcrumbs = buildBreadcrumbs(laName ?? "")
                    .Prepend(GetChildPageBreadcrumbs(laCode, laName ?? ""))
                from downloadDataPage in GetDownloadDataPage(
                    breadcrumbs,
                    nameof(DownloadDataSelectYear),
                    GetSubNavigation(Request.Path),
                    GetSideNavigation(Request.Path))
                select new DownloadDataSelectYearModel(breadcrumbs, downloadDataPage.SubNavigation,
                    downloadDataPage.SideNavigation,
                    availableDownloads.AvailableDates, downloadDataPage.Title, downloadDataPage.SubTitle,
                    downloadDataPage.ContentTitle, downloadDataPage.ContentTitleCaption, downloadDataPage.ControllerName, actionName);
        }

        protected Task<Result<DownloadDataSelectFilesModel>> DownloadDataSelectFiles(string laCode,
            Optional<int> selectedYear,
            Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs,
            string actionName)
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, selectedYear)
                let breadcrumbs = buildBreadcrumbs(laName ?? "")
                    .Prepend(GetChildPageBreadcrumbs(laCode, laName ?? ""))
                from downloadDataPage in GetDownloadDataPage(
                    breadcrumbs,
                    nameof(DownloadDataSelectFiles),
                    GetSubNavigation(Request.Path),
                    GetSideNavigation(Request.Path))
                select new DownloadDataSelectFilesModel(breadcrumbs, downloadDataPage.SubNavigation,
                    downloadDataPage.SideNavigation,
                    availableDownloads.Downloads, downloadDataPage.Title, downloadDataPage.SubTitle,
                    downloadDataPage.ContentTitle, downloadDataPage.ContentTitleCaption,
                    downloadDataPage.ControllerName, actionName);
        }

        protected Task<Result<DownloadDataSelectFormatModel>> DownloadDataSelectFormat(string laCode, int selectedYear,
            List<string> selectedFiles, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs,
            string linkText, string actionName, string fileType = "CSV")
        {
            return
                from laName in GetLocalAuthorityName(laCode)
                let breadcrumbs = buildBreadcrumbs(laName ?? "")
                    .Prepend(GetChildPageBreadcrumbs(laCode, laName ?? ""))
                from downloadDataPage in GetDownloadDataPage(
                    breadcrumbs,
                    nameof(DownloadDataSelectFormat),
                    GetSubNavigation(Request.Path),
                    GetSideNavigation(Request.Path))
                select new DownloadDataSelectFormatModel(breadcrumbs, downloadDataPage.SubNavigation,
                    downloadDataPage.SideNavigation, downloadDataPage.Title, downloadDataPage.SubTitle,
                    downloadDataPage.ContentTitle, downloadDataPage.ContentTitleCaption, downloadDataPage.ControllerName, actionName,
                    linkText, fileType, selectedFiles, "LA");
        }

        protected abstract BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName);

        protected abstract IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName);

        protected abstract NavigationViewModel GetSubNavigation(
            PathString requestPath);

        protected abstract NavigationViewModel GetSideNavigation(
            PathString requestPath);

        protected abstract Task<Result<BaseDownloadDataModel>> GetDownloadDataPage(
            BreadcrumbTrailViewModel breadcrumb,
            string currentActionName,
            NavigationViewModel? subNavigation = null,
            NavigationViewModel? sideNavigation = null);

        protected abstract Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName,
            BreadcrumbTrailViewModel breadcrumbs);

        protected virtual Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return
                from la in _api.GetLocalAuthority(new(laCode))
                select string.IsNullOrWhiteSpace(la.Name)
                    ? "Missing local authority name"
                    : la.Name;
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentTemplateId,
            string? revision)
        {
            var model =
                from template in _api.ViewContentTemplate(new(contentTemplateId, Optional.FromNullable(revision)))
                select ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template);

            return model
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected virtual Task<Result<AvailableDownloadsViewModel>> GetAvailableLaDownloads(string laCode,
            Optional<int> year)
        {
            return
                from downloads in _api.GetAvailableLaDownloads(new GetAvailableLADownloadsRequest(laCode, year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }

        protected virtual Task<Result<ActionResult>> GetDownloadsAsZipFile(FileType fileType, List<string> fileIds)
        {
            return
                from response in _api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, fileIds))
                select (ActionResult)new FileStreamResult(response.Content, response.ContentType)
                {
                    FileDownloadName = response.FileName
                };
        }
    }
}