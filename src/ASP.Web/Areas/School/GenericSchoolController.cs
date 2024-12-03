using ASP.Application;
using ASP.Core;
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
    [Route("school/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolController : BaseSchoolController
    {
        public GenericSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string urn, string? revision)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail(laCode, laName),
                            establishmentDetails.Name
                        ),
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
        public Task<IActionResult> OtherReports(string urn, string? revision)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name),
                            "Other reports"
                        ),
                        "Other reports",
                        establishmentDetails.Name,
                        GetSubNavigation(urn)
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name),
                            "Useful links"
                        ),
                        "Useful links",
                        establishmentDetails.Name,
                        GetSubNavigation(urn)
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadDataSelectYear(string urn)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(establishmentDetails.Urn, Optional<int>.None)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name),
                            "Download data"
                        ),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(urn),
                        GetDownloadDataSideNavigation(urn, establishmentDetails.Name),
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
        public async Task<IActionResult> DownloadDataSelectYear(string urn, int? selectedYear)
        {
            if (!selectedYear.HasValue)
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                    Constants.AcademicYearToDownldValidationErrorMessage);

                // Reload the view with the error
                return await DownloadDataSelectYear(urn);
            }

            // Redirect to files selection if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFiles),
                new { urn, selectedYear });
        }

        [HttpGet("download-data/select-files")]
        public Task<IActionResult> DownloadDataSelectFiles(string urn, int? selectedYear)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(establishmentDetails.Urn, Optional<int>.None)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name).Concat([
                                new("Download data", Action(nameof(DownloadDataSelectYear),
                                    new { urn, selectedYear = "", selectedFiles = "" }))
                            ]),
                            "Data files available for download"
                        ),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(urn),
                        GetDownloadDataSideNavigation(urn, establishmentDetails.Name),
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
        public async Task<IActionResult> DownloadDataSelectFiles(string urn, int? selectedYear, List<string> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                    Constants.DataFilesAvialableForDownlodValidationErrorMessage);

                return await DownloadDataSelectFiles(urn, selectedYear);
            }

            // Redirect to data select format page if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFormat),
                new { urn, selectedYear, selectedFiles });
        }

        [HttpGet("download-data/select-format")]
        public Task<IActionResult> DownloadDataSelectFormat(string urn, int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from availableDownloads in GetAvailableDownloads(establishmentDetails.Urn, Optional.FromNullable(selectedYear))
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new Web.Shared.PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name).Concat([
                                new("Download data", Action(nameof(DownloadDataSelectYear),
                                    new { urn, selectedYear = "", selectedFiles = "" })),
                                new("Data files available for download", Action(nameof(DownloadDataSelectFiles),
                                    new { urn, selectedYear, selectedFiles = "" }))
                            ]),
                            $"Download {establishmentDetails.Name} data"
                        ),
                        "Download data",
                        establishmentDetails.Name,
                        GetSubNavigation(urn),
                        GetDownloadDataSideNavigation(urn, establishmentDetails.Name),
                        $"Download {establishmentDetails.Name} data",
                        $"{establishmentDetails.Name} data"
                ))
                select new SchoolDownloadDataSelectFormatViewModel(
                    schoolPage,
                    new DownloadDataSelectFormatModel(
                        "school",
                        [("Data in CSV format", Url.Action(nameof(DownloadDataAsZip), 
                            new { urn, fileType = "CSV", selectedFiles }))],
                        Action(nameof(DownloadDataSelectYear), 
                            new { urn, selectedYear = "", selectedFiles = "" })
                    )
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/download-as-zip")]
        public Task<IActionResult> DownloadDataAsZip(string urn, FileType fileType, List<string> selectedFiles)
        {
            return base.DownloadDataAsZip(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail(string laCode, string laName) => 
            [
                new("All local authorities", $"/local-authorities/"),
                new(laName, $"/local-authority/{laCode}/"),
                new("All schools", $"/local-authority/{laCode}/schools/")
            ];

        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail(string laCode, string laName, string urn, string name) =>
            GetBaseBreadcrumbTrail(laCode, laName).Concat([
                new(name, Action(nameof(LandingPage),
                    new { urn })),
            ]);

        private NavigationViewModel GetSubNavigation(string urn) =>
            new([
                new("Download data", Action(nameof(DownloadDataSelectYear),
                    new { urn, selectedYear = "", selectedFiles = "" }), Request.Path),
                new("Other reports", Action(nameof(OtherReports),
                    new { urn }), Request.Path),
                new("Useful links", Action(nameof(UsefulLinks),
                    new { urn }), Request.Path)
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string urn, string name) =>
            new([
                new($"{name} data", Action(nameof(DownloadDataSelectYear),
                    new { urn, selectedYear = "", selectedFiles = "" }), Request.Path)
            ]);

        private string Action(string action, object? values = null) => Url.Action(action, values) ?? "";
    }
}
