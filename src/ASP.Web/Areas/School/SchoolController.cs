using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Downloads.DownloadAsZip;
using ASP.Application.UseCases.Downloads.GetAvailableSchoolDownloads;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    public abstract class SchoolController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        protected readonly IAspApiClient _api;
        protected readonly IHostEnvironment _hostEnvironment;

        protected SchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        protected Task<Result<SchoolLandingPageViewModel>> LandingPage(string urn, string? revision)
        {
            return 
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let breadcrumbs = GetLandingPageBreadcrumbs(establishmentDetails.Name ?? "")
                from schoolPage in GetSchoolPage(
                    establishmentDetails, 
                    breadcrumbs)
                select new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate);
        }

        protected Task<Result<SchoolContentPageViewModel>> OtherReports(string urn, string? revision, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs)
        {
            return 
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let breadcrumbs = buildBreadcrumbs(establishmentDetails.Name ?? "")
                    .Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? ""))
                from schoolPage in GetSchoolPage(
                    establishmentDetails,
                    breadcrumbs,
                    GetSubNavigation(establishmentDetails, Request.Path))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate);
        }

        protected Task<Result<SchoolContentPageViewModel>> UsefulLinks(string urn, string? revision, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs)
        {
            return 
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision)
                let breadcrumbs = buildBreadcrumbs(establishmentDetails.Name ?? "")
                    .Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? ""))
                from schoolPage in GetSchoolPage(
                    establishmentDetails, 
                    breadcrumbs,
                    GetSubNavigation(establishmentDetails, Request.Path))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate);
        }

        protected Task<Result<SchoolDownloadsViewModel>> DownloadDataSelectYear(string urn, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs)
        { 
            return 
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(establishmentDetails.Urn, Optional<int>.None)
                let breadcrumbs = buildBreadcrumbs(establishmentDetails.Name ?? "")
                    .Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? ""))
                from schoolPage in GetSchoolPage(
                    establishmentDetails, 
                    breadcrumbs,
                    GetSubNavigation(establishmentDetails, Request.Path),
                    GetSideNavigation(establishmentDetails, Request.Path))
                select new SchoolDownloadsViewModel(
                    schoolPage,
                    availableDownloads);
        }

        protected Task<Result<SchoolDownloadsViewModel>> DownloadDataSelectFiles(string urn, Optional<int> selectedYear, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs)
        {
            return 
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(establishmentDetails.Urn, selectedYear)
                let breadcrumbs = buildBreadcrumbs(establishmentDetails.Name ?? "")
                    .Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? ""))
                from schoolPage in GetSchoolPage(
                    establishmentDetails, 
                    breadcrumbs,
                    GetSubNavigation(establishmentDetails, Request.Path),
                    GetSideNavigation(establishmentDetails, Request.Path))
                select new SchoolDownloadsViewModel(
                    schoolPage,
                    availableDownloads
                );
        }

        protected Task<Result<SchoolDownloadsSelectFormatViewModel>> DownloadDataSelectFormat(string urn, int selectedYear, List<string> selectedFiles, Func<string, BreadcrumbTrailViewModel> buildBreadcrumbs)
        {
            return
                from establishmentDetails in GetEstablishmentDetails(urn)
                let breadcrumbs = buildBreadcrumbs(establishmentDetails.Name ?? "")
                    .Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? ""))
                from schoolPage in GetSchoolPage(
                    establishmentDetails, 
                    breadcrumbs,
                    GetSubNavigation(establishmentDetails, Request.Path),
                    GetSideNavigation(establishmentDetails, Request.Path))
                select new SchoolDownloadsSelectFormatViewModel(
                    schoolPage,
                    selectedFiles);
        }

        protected abstract Task<Result<SchoolPageViewModel>> GetSchoolPage(
            EstablishmentDetailsViewModel establishmentDetails, 
            BreadcrumbTrailViewModel breadcrumb, 
            NavigationViewModel? subNavigation = null, 
            NavigationViewModel? sideNavigation = null);
        
        protected abstract BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName);
        
        protected abstract IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string urn, string schoolName);
        
        protected abstract NavigationViewModel GetSubNavigation(
            EstablishmentDetailsViewModel establishmentDetails, 
            PathString requestPath);
        
        protected abstract NavigationViewModel GetSideNavigation(
            EstablishmentDetailsViewModel establishmentDetails, 
            PathString requestPath);

        protected virtual Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return 
                from establishmentDetails in _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                select EstablishmentDetailsViewModel.FromEstablishmentDetails(establishmentDetails);
        }

        protected virtual Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentId, string? revision)
        {
            return (
                from template in _api.ViewContentTemplate(new ViewContentTemplateRequest(contentId, Optional.FromNullable(revision)))
                select ContentTemplateViewModel.FromTemplate(contentId, revision, template)
            ).DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected virtual Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string urn, Optional<int> year)
        {
            return 
                from downloads in _api.GetAvailableSchoolDownloads(new GetAvailableSchoolDownloadsRequest(urn, year))
                select AvailableDownloadsViewModel.FromAvailableDownloads(downloads);
        }

        protected virtual Task<Result<ActionResult>> GetDownloadsAsZipFile(FileType fileType, List<string> fileIds)
        {
            return _api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, fileIds));
        }
    }
}
