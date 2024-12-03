using ASP.Application;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.Shared.DownloadData.SelectFiles;
using ASP.Web.Areas.Shared.DownloadData.SelectFormat;
using ASP.Web.Areas.Shared.DownloadData.SelectYear;
using ASP.Web.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToAllLocalAuthorities)]
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class GenericLocalAuthorityController : BaseLocalAuthorityController
    {
        public GenericLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment)
            : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string laCode, string? revision)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetBaseBreadcrumbTrail(),
                        laName
                    ),
                    laName,
                    $"All schools within {laName}"
                )
                select new LocalAuthorityContentPageViewModel(
                    page,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadDataSelectYear(string laCode)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, Optional<int>.None)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail(laCode, laName),
                        "Download data"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(laCode),
                    GetDownloadDataSideNavigation(laCode),
                    "Dates available for download",
                    "Pupil level and aggregated LA data"
                )
                select new LocalAuthorityDownloadDataSelectYearViewModel(
                    page,
                    new DownloadDataSelectYearModel(availableDownloads.AvailableDates)
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data")]
        public async Task<IActionResult> DownloadDataSelectYear(string laCode, int? selectedYear)
        {
            if (!selectedYear.HasValue)
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                    Constants.AcademicYearToDownldValidationErrorMessage);

                return await DownloadDataSelectYear(laCode);
            }

            // Redirect to files selection if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFiles),
                new { laCode, selectedYear });
        }

        [HttpGet("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(string laCode, int? selectedYear)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, Optional.FromNullable(selectedYear))
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail(laCode, laName).Concat([
                            new("Download data", Action(nameof(DownloadDataSelectYear),
                                new { laCode, selectedYear = "", selectedFiles = "" })),
                        ]),
                        "Data files available for download"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(laCode),
                    GetDownloadDataSideNavigation(laCode),
                    "Data files available for download",
                    "Pupil level and aggregated LA data"
                )
                select new LocalAuthorityDownloadDataSelectFilesViewModel(
                    page,
                    new DownloadDataSelectFilesModel(availableDownloads.Downloads)
                );

            return await result.ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(string laCode, int? selectedYear, List<string> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                    Constants.DataFilesAvialableForDownlodValidationErrorMessage);

                return await DownloadDataSelectFiles(laCode, selectedYear);
            }

            // Redirect to data select format page if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFormat),
                new { laCode, selectedYear, selectedFiles });
        }

        [HttpGet("download-data/select-format")]
        public Task<IActionResult> DownloadDataSelectFormat(string laCode, int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail(laCode, laName).Concat([
                            new("Download data", Action(nameof(DownloadDataSelectYear),
                                new { laCode, selectedYear = "", selectedFiles = "" })),
                            new("Data files available for download", Action(nameof(DownloadDataSelectFiles),
                                new { laCode, selectedYear, selectedFiles = "" })),
                        ]),
                        "Download pupil level and aggregated LA data"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(laCode),
                    GetDownloadDataSideNavigation(laCode),
                    "Download pupil level and aggregated LA data",
                    "Pupil level and aggregated LA data"
                )
                select new LocalAuthorityDownloadDataSelectFormatViewModel(
                    page,
                    new DownloadDataSelectFormatModel(
                        "LA",
                        [("Data in CSV format", Action(nameof(DownloadDataAsZip), 
                            new { laCode, fileType = "CSV", selectedFiles }))],
                        Action(nameof(DownloadDataSelectYear), 
                            new { laCode, selectedYear = "", selectedFiles = "" })
                    )
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/download-as-zip")]
        public Task<IActionResult> DownloadDataAsZip(string laCode, FileType fileType, List<string> selectedFiles)
        {
            return base.DownloadDataAsZip(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet("download-data/individual-school-data")]
        public Task<IActionResult> IndividualSchoolData(string laCode)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail(laCode, laName).Concat([
                            new("Download data", Action(nameof(DownloadDataSelectYear),
                                new { selectedYear = "", selectedFiles = "" })),
                        ]),
                        "Search for a school"
                    ),
                    "Download data",
                    "Individual school data",
                    GetSubNavigation(laCode),
                    GetDownloadDataSideNavigation(laCode),
                    "Search for a school",
                    "Individual school data"
                )
                select new LocalAuthorityDownloadDataSelectYearViewModel(
                    page,
                    new DownloadDataSelectYearModel([])
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() => 
            [
                new("All local authorities", $"/local-authorities/")
            ];

        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail(string laCode, string laName) =>
            GetBaseBreadcrumbTrail().Concat([
                new(laName, $"/local-authority/{laCode}/"),
            ]);

        private NavigationViewModel GetSubNavigation(string laCode) =>
            new([
                new("Download data", Action(nameof(DownloadDataSelectYear),
                    new { laCode, selectedYear = "", selectedFiles = "" }), Request.Path),
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string laCode) =>
            new([
                new("Pupil level and aggregated LA data", Action(nameof(DownloadDataSelectYear),
                    new { laCode, selectedYear = "", selectedFiles = "" }), Request.Path),
                new("Individual school data", Action(nameof(IndividualSchoolData),
                    new { laCode }), Request.Path)
            ]);

        private string Action(string action, object? values = null) => Url.Action(action, values) ?? "";
    }
}