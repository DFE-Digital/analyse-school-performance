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

        protected async Task<Result<SchoolLandingPageViewModel>> LandingPage(string urn, string? revision)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetSchoolPage(establishmentDetails, GetLandingPageBreadcrumbs(establishmentDetails.Name))
                .Map(schoolPage => new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolContentPageViewModel>> OtherReports(string urn, string? revision, Func<string, BreadcrumbTrailViewModel> breadcrumbs)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)       
                .Then(contentTemplate => GetSchoolPage(establishmentDetails,
                                                  breadcrumbs(establishmentDetails.Name ?? "").Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? "")),
                                                  GetSubNavigation(establishmentDetails, Request.Path))
                .Map(schoolPage => new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolContentPageViewModel>> UsefulLinks(string urn, string? revision, Func<string, BreadcrumbTrailViewModel> breadcrumbs)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetContentTemplate(USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision)
                .Then(contentTemplate => GetSchoolPage(establishmentDetails, 
                                                  breadcrumbs(establishmentDetails.Name ?? "").Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? "")),
                                                  GetSubNavigation(establishmentDetails, Request.Path))
                .Map(schoolPage => new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                ))));
        }

        protected async Task<Result<SchoolDownloadsViewModel>> DownloadDataSelectYear(string urn, Func<string, BreadcrumbTrailViewModel> breadcrumbs)
        { 
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetAvailableDownloads(establishmentDetails.Urn, Optional<int>.None)
                .Then(availableDownloads => GetSchoolPage(establishmentDetails, 
                                                  breadcrumbs(establishmentDetails.Name ?? "").Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? "")),
                                                  GetSubNavigation(establishmentDetails, Request.Path),
                                                  GetSideNavigation(establishmentDetails, Request.Path))
                .Map(schoolPage => new SchoolDownloadsViewModel(
                    schoolPage,
                    availableDownloads
                ))));
        }

        protected async Task<Result<SchoolDownloadsViewModel>> DownloadDataSelectFiles(string urn, Optional<int> selectedYear, Func<string, BreadcrumbTrailViewModel> breadcrumbs)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails => GetAvailableDownloads(establishmentDetails.Urn, selectedYear)
                .Then(availableDownloads => GetSchoolPage(establishmentDetails, 
                                                          breadcrumbs(establishmentDetails.Name ?? "").Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? "")),
                                                          GetSubNavigation(establishmentDetails, Request.Path),
                                                          GetSideNavigation(establishmentDetails, Request.Path))
                .Map(schoolPage => new SchoolDownloadsViewModel(
                    schoolPage,
                    availableDownloads
                ))));
        }

        protected async Task<Result<SchoolDownloadsSelectFormatViewModel>> DownloadDataSelectFormat(string urn, int selectedYear, List<string> selectedFiles, Func<string, BreadcrumbTrailViewModel> breadcrumbs)
        {
            return await GetEstablishmentDetails(urn)
                .Then(establishmentDetails=> GetSchoolPage(establishmentDetails, 
                                                 breadcrumbs(establishmentDetails.Name ?? "").Prepend(GetChildPageBreadcrumbs(urn, establishmentDetails.Name ?? "")),
                                                 GetSubNavigation(establishmentDetails, Request.Path),
                                                 GetSideNavigation(establishmentDetails, Request.Path))
                .Map(schoolPage => new SchoolDownloadsSelectFormatViewModel(
                    schoolPage,
                    selectedFiles
                )));
        }

        protected abstract Task<Result<SchoolPageViewModel>> GetSchoolPage(EstablishmentDetailsViewModel establishmentDetails, 
                                                                           BreadcrumbTrailViewModel breadcrumb, 
                                                                           NavigationViewModel? subNavigation = null, 
                                                                           NavigationViewModel? sideNavigation = null);
        protected abstract BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName);
        protected abstract IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string urn, string schoolName);
        protected abstract NavigationViewModel GetSubNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath);
        protected abstract NavigationViewModel GetSideNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath);


        protected virtual async Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return await _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails);
        }

        protected virtual async Task<Result<ContentTemplateViewModel>> GetContentTemplate(string contentId, string? revision)
        {
            return await _api.ViewContentTemplate(new ViewContentTemplateRequest(contentId, Optional.FromNullable(revision)))
                .Map(template => ContentTemplateViewModel.FromTemplate(contentId, revision, template))
                .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel());
        }

        protected virtual async Task<Result<AvailableDownloadsViewModel>> GetAvailableDownloads(string urn, Optional<int> year)
        {
            return await _api.GetAvailableSchoolDownloads(new GetAvailableSchoolDownloadsRequest(urn, year))
                .Map(response => AvailableDownloadsViewModel.FromAvailableDownloads(response));
        }

        protected virtual async Task<Result<ActionResult>> GetDownloadsAsZipFile(FileType fileType, List<string> fileIds)
        {
            return await _api.DownloadAsZipFile(new DownloadAsZipFileRequest(fileType, fileIds));
        }
    }
}
