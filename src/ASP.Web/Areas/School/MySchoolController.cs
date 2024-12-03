using ASP.Application;
using ASP.Core;
using ASP.Core.Authorization;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.DownloadData.SelectFiles;
using ASP.Web.Areas.Shared.DownloadData.SelectFormat;
using ASP.Web.Areas.Shared.DownloadData.SelectYear;
using ASP.Web.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchool)]
    public class MySchoolController : BaseSchoolController
    {
        public MySchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetBaseBreadcrumbTrail(), "My school"),
                        "My school",
                        establishmentDetails.Name
                ))
                select new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail(), "Other reports"),
                        "Other reports",
                        establishmentDetails.Name,
                        GetSubNavigation()
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail(), "Useful links"),
                        "Useful links",
                        establishmentDetails.Name,
                        GetSubNavigation()
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadDataSelectYear()
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(urn, Optional<int>.None)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail(), "Download data"),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(),
                        GetDownloadDataSideNavigation(establishmentDetails.Name),
                        "Dates available for download",
                        $"{establishmentDetails.Name} data"
                ))
                select new SchoolDownloadDataSelectYearViewModel(
                    schoolPage,
                    new DownloadDataSelectYearModel(availableDownloads.AvailableDates)
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data")]
        public async Task<IActionResult> DownloadDataSelectYear(int? selectedYear)
        {
            if (!selectedYear.HasValue)
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                    Constants.AcademicYearToDownldValidationErrorMessage);

                // Reload the view with the error
                return await DownloadDataSelectYear();
            }

            // Redirect to files selection if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFiles),
                new { selectedYear });
        }

        [HttpGet("download-data/select-files")]
        public Task<IActionResult> DownloadDataSelectFiles(int? selectedYear)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(urn, Optional.FromNullable(selectedYear))
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail().Concat([
                            new("Download data", Action(nameof(DownloadDataSelectYear), 
                                new { selectedYear = "", selectedFiles = "" })),
                        ]), "Data files available for download"),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(),
                        GetDownloadDataSideNavigation(establishmentDetails.Name),
                        "Data files available for download",
                        $"{establishmentDetails.Name} data"
                ))
                select new SchoolDownloadDataSelectFilesViewModel(
                    schoolPage,
                    new DownloadDataSelectFilesModel(availableDownloads.Downloads)
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(int? selectedYear, List<string> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                    Constants.DataFilesAvialableForDownlodValidationErrorMessage);

                return await DownloadDataSelectFiles(selectedYear);
            }

            // Redirect to data select format page if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFormat),
                new { selectedYear, selectedFiles });
        }

        [HttpGet("download-data/select-format")]
        public Task<IActionResult> DownloadDataSelectFormat(int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail().Concat([
                            new("Download data", Action(nameof(DownloadDataSelectYear), 
                                new { selectedYear = "", selectedFiles = "" })),
                            new("Data files available for download", Action(nameof(DownloadDataSelectFiles), 
                                new { selectedYear, selectedFiles = "" }))
                        ]), $"Download {establishmentDetails.Name} data"),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(),
                        GetDownloadDataSideNavigation(establishmentDetails.Name),
                        $"Download {establishmentDetails.Name} data",
                        $"{establishmentDetails.Name} data"
                ))
                select new SchoolDownloadDataSelectFormatViewModel(
                    schoolPage,
                    new DownloadDataSelectFormatModel(
                        "school",
                        [("Data in CSV format", Action(nameof(DownloadDataAsZip), 
                            new { fileType = "CSV", selectedFiles }))],
                        Action(nameof(DownloadDataSelectYear), 
                            new { selectedYear = "", selectedFiles = "" })
                    )
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/download-as-zip")]
        public new Task<IActionResult> DownloadDataAsZip(FileType fileType, List<string> selectedFiles)
        {
            return base.DownloadDataAsZip(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        protected override Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return base.GetEstablishmentDetails(urn)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() => [];
        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail() => 
            GetBaseBreadcrumbTrail().Concat([
                new("My school", Action(nameof(LandingPage))),
            ]);

        private NavigationViewModel GetSubNavigation() =>
            new([
                new("Download data", Action(nameof(DownloadDataSelectYear), 
                    new { selectedYear = "", selectedFiles = "" }), Request.Path),
                new("Other reports", Action(nameof(OtherReports)), Request.Path),
                new("Useful links", Action(nameof(UsefulLinks)), Request.Path)
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string name) =>
            new([
                new($"{name} data", Action(nameof(DownloadDataSelectYear), 
                    new { selectedYear = "", selectedFiles = "" }), Request.Path)
            ]);

        private string Action(string action, object? values = null) => Url.Action(action, values) ?? "";
    }
}