using ASP.Application;
using ASP.Core;
using ASP.Core.Authorization;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.Shared.DownloadData;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToMyLocalAuthority)]
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class MyLocalAuthorityController : LocalAuthorityController
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
                from model in base.LandingPage(laCode, revision)
                select model;

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public async Task<IActionResult> DownloadDataSelectYear()
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.DownloadDataSelectYear(laCode, _ => new([],
                    "Download data"), nameof(DownloadDataSelectYear))
                select model;

            return await result.ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data")]
        public async Task<IActionResult> DownloadDataSelectYear(int? selectedYear)
        {
            if (!selectedYear.HasValue)
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedYear,
                    Constants.AcademicYearToDownldValidationErrorMessage);

                // Reload the view with the error
                var result =
                    from laCode in User.GetLocalAuthorityCode()
                    from model in base.DownloadDataSelectYear(laCode, _ => new([],
                        "Download data"), nameof(DownloadDataSelectYear))
                    select model;

                return await result.ToActionResult(View, _hostEnvironment);
            }

            // Redirect to files selection if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFiles),
                new { selectedYear = selectedYear });
        }

        [HttpGet("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(int? selectedYear)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.DownloadDataSelectFiles(
                    laCode,
                    Optional.FromNullable(selectedYear),
                    schoolName => new([
                        new("Download data", $"/my-local-authority/download-data/")
                    ], "Data files available for download"), nameof(DownloadDataSelectFiles))
                select model;

            return await result.ToActionResult(View, _hostEnvironment);
        }

        [HttpPost("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.DownloadDataSelectFiles(
                    laCode,
                    Optional.FromNullable(selectedYear),
                    schoolName => new([
                        new("Download data", $"/my-local-authority/download-data/")
                    ], "Data files available for download"), nameof(DownloadDataSelectFiles))
                select model;

            if (!selectedFiles.Any())
            {
                ModelState.AddModelError(Constants.ModelErrorKeySelectedFiles,
                    Constants.DataFilesAvialableForDownlodValidationErrorMessage);
                return await result.ToActionResult(View, _hostEnvironment);
            }

            // Redirect to data select format page if year is valid
            return RedirectToAction(nameof(DownloadDataSelectFormat),
                new { selectedYear = selectedYear, selectedFiles = selectedFiles });
        }

        [HttpGet("download-data/select-format")]
        public Task<IActionResult> DownloadDataSelectFormat(int selectedYear, List<string> selectedFiles)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.DownloadDataSelectFormat(
                    laCode,
                    selectedYear,
                    selectedFiles,
                    schoolName => new([
                            new("Download data", $"/my-local-authority/download-data/"),
                            new("Data files available for download",
                                $"/my-local-authority/download-data/select-files/?selectedYear={selectedYear}")
                        ], $"Download pupil level and aggregated LA data"),
                    "Data in CSV format", nameof(DownloadDataAsZip))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/download-as-zip")]
        public Task<IActionResult> DownloadDataAsZip(FileType fileType, List<string> selectedFiles)
        {
            return base.GetDownloadsAsZipFile(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName) => new(
            [],
            "My local authority");

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName) =>
        [
            new("My local authority", $"/my-local-authority/")
        ];

        protected override NavigationViewModel GetSubNavigation(
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("download-data", "Download data", $"/my-local-authority/download-data/", requestPath)
            ]));
        }

        protected override NavigationViewModel GetSideNavigation(
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("pupil-level-and-aggregated-la-data", $"Pupil level and aggregated LA data",
                    $"/my-local-authority/download-data/", requestPath),
                new("individual-school-data", $"Individual school data", $"/my-local-authority/individual-school-data/", requestPath)
            ]));
        }

        protected override Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return base.GetLocalAuthorityName(laCode)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName,
            BreadcrumbTrailViewModel breadcrumbs)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                "My local authority",
                laName,
                breadcrumbs
            );

            return Task.FromResult(Result.Success(viewModel));
        }

        protected override Task<Result<BaseDownloadDataModel>> GetDownloadDataPage(
            BreadcrumbTrailViewModel breadcrumb,
            string currentActionName,
            NavigationViewModel? subNavigation = null,
            NavigationViewModel? sideNavigation = null)
        {
            var title = "Download data";
            var subtitle = $@"Pupil level and aggregated LA data";
            
            var (contentTitle, contentTitleCaption) = currentActionName switch
            {
                nameof(DownloadDataSelectYear) => (
                    "Dates available for download",
                    "Pupil level and aggregated LA data"
                ),
                nameof(DownloadDataSelectFiles) => (
                    "Data files available for download",
                    "Pupil level and aggregated LA data"
                ),
                nameof(DownloadDataSelectFormat) => (
                    "Download pupil level and aggregated LA data",
                    "Pupil level and aggregated LA data"
                ),
                _ => ("", "") // Default case
            };

            var downloadDataPage = new BaseDownloadDataModel(
                breadcrumb,
                subNavigation,
                sideNavigation,
                title,
                subtitle,
                contentTitle,
                contentTitleCaption,
                "MyLocalAuthority",
                ""
            );
            return Task.FromResult(Result.Success(downloadDataPage));
        }
    }
}