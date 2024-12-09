using ASP.Application;
using ASP.Core;
using ASP.Core.Authorization;
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
    [Authorize(Policy = Policy.AccessToMyLocalAuthority)]
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class MyLocalAuthorityController : BaseLocalAuthorityController
    {
        public MyLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetBaseBreadcrumbTrail(), 
                        "My local authority"
                    ),
                    "My local authority",
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
        public IActionResult DownloadData()
        {
            return RedirectToActionPermanent(nameof(DownloadPupilLevelAggregatedLADataSelectYear));
        }

        [HttpGet("download-data/pupil-level-aggregated-la-data")]
        public Task<IActionResult> DownloadPupilLevelAggregatedLADataSelectYear()
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, Optional<int>.None)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail(),
                        "Download data"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(),
                    GetDownloadDataSideNavigation(),
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

        [HttpPost("download-data/pupil-level-aggregated-la-data")]
        public async Task<IActionResult> DownloadPupilLevelAggregatedLADataSelectYear(int? selectedYear)
        {
            if (!selectedYear.HasValue)
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                    Constants.AcademicYearToDownldValidationErrorMessage);

                return await DownloadPupilLevelAggregatedLADataSelectYear();
            }

            // Redirect to files selection if year is valid
            return RedirectToAction(nameof(DownloadPupilLevelAggregatedLADataSelectFiles),
                new { selectedYear });
        }

        [HttpGet("download-data/pupil-level-aggregated-la-data/select-files")]
        public Task<IActionResult> DownloadPupilLevelAggregatedLADataSelectFiles(int? selectedYear)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from availableDownloads in GetAvailableLaDownloads(laCode, Optional.FromNullable(selectedYear))
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail().Concat([
                            new("Download data", Action(nameof(DownloadData)))
                        ]), 
                        "Data files available for download"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(),
                    GetDownloadDataSideNavigation(),
                    "Data files available for download",
                    "Pupil level and aggregated LA data"
                )
                select new LocalAuthorityDownloadDataSelectFilesViewModel(
                    page,
                    new DownloadDataSelectFilesModel(availableDownloads.Downloads)
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data/pupil-level-aggregated-la-data/select-files")]
        public async Task<IActionResult> DownloadPupilLevelAggregatedLADataSelectFiles(int? selectedYear, List<string> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                    Constants.DataFilesAvialableForDownlodValidationErrorMessage);

                return await DownloadPupilLevelAggregatedLADataSelectFiles(selectedYear);
            }

            // Redirect to data select format page if year is valid
            return RedirectToAction(nameof(DownloadPupilLevelAggregatedLADataSelectFormat),
                new { selectedYear, selectedFiles });
        }

        [HttpGet("download-data/pupil-level-aggregated-la-data/select-format")]
        public Task<IActionResult> DownloadPupilLevelAggregatedLADataSelectFormat(int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail().Concat([
                            new("Download data", Action(nameof(DownloadData))),
                            new("Data files available for download", Action(nameof(DownloadPupilLevelAggregatedLADataSelectFiles),
                                new { selectedYear, selectedFiles = "" }))
                        ]),
                        "Download pupil level and aggregated LA data"
                    ),
                    "Download data",
                    "Pupil level and aggregated LA data",
                    GetSubNavigation(),
                    GetDownloadDataSideNavigation(),
                    "Download pupil level and aggregated LA data",
                    "Pupil level and aggregated LA data"
                )
                select new LocalAuthorityDownloadDataSelectFormatViewModel(
                    page,
                    new DownloadDataSelectFormatModel(
                        "LA",
                        [("Data in CSV format", Url.Action(nameof(DownloadDataAsZip), 
                            new { fileType = "CSV", selectedFiles }))],
                        Action(nameof(DownloadPupilLevelAggregatedLADataSelectYear), 
                            new { selectedYear = "", selectedFiles = "" })
                    )
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/individual-school-data")]
        public Task<IActionResult> DownloadIndividualSchoolDataSelectYear()
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(
                        GetChildPageBaseBreadcrumbTrail().Concat([
                            new("Download data", Action(nameof(DownloadData)))
                        ]),
                        "Search for a school"
                    ),
                    "Download data",
                    "Individual school data",
                    GetSubNavigation(),
                    GetDownloadDataSideNavigation(),
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

        [HttpGet("download-data/download-as-zip")]
        public new Task<IActionResult> DownloadDataAsZip(FileType fileType, List<string> selectedFiles)
        {
            return base.DownloadDataAsZip(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        protected override Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return base.GetLocalAuthorityName(laCode)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() => [];
        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail() =>
            GetBaseBreadcrumbTrail().Concat([
                new("My local authority", Action(nameof(LandingPage))),
            ]);

        private NavigationViewModel GetSubNavigation() =>
            new([
                new("Download data", Action(nameof(DownloadData)), Request.Path),
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation() =>
            new([
                new("Pupil level and aggregated LA data", Action(nameof(DownloadPupilLevelAggregatedLADataSelectYear), 
                    new { selectedYear = "", selectedFiles = "" }), Request.Path),
                new("Individual school data", Action(nameof(DownloadIndividualSchoolDataSelectYear)), Request.Path)
            ]);

        private string Action(string action, object? values = null) => Url.Action(action, values) ?? "";
    }
}