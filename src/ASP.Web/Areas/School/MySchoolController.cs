using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core;
using ASP.Core.Authorization;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.DownloadData;
using ASP.Web.Areas.Shared.Navigation;
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
    public class MySchoolController : SchoolController
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
                from model in base.LandingPage(urn, revision)
                select model;
            
            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.OtherReports(urn, revision, schoolName => new([], "Other reports"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.UsefulLinks(urn, revision, _ => new([], "Useful links"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }
        [HttpGet("download-data")]
        public async Task<IActionResult> DownloadDataSelectYear()
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectYear(urn, _ => new([], 
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
                    from urn in User.GetEstablishmentUrn()
                    from model in base.DownloadDataSelectYear(urn, _ => new([], 
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
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectFiles(
                    urn,
                    Optional.FromNullable(selectedYear),
                    schoolName => new([
                        new("Download data", $"/my-school/download-data/")
                    ], "Data files available for download"), nameof(DownloadDataSelectFiles))
                select model;
            
            return await result.ToActionResult(View, _hostEnvironment);
        }
        
        [HttpPost("download-data/select-files")]
        public async Task<IActionResult> DownloadDataSelectFiles(int? selectedYear, List<string> selectedFiles)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectFiles(
                    urn,
                    Optional.FromNullable(selectedYear),
                    schoolName => new([
                        new("Download data", $"/my-school/download-data/")
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
                new { selectedYear = selectedYear, selectedFiles= selectedFiles });
        }

        [HttpGet("download-data/select-format")]
        public Task<IActionResult> DownloadDataSelectFormat(int selectedYear, List<string> selectedFiles)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectFormat(
                    urn,
                    selectedYear,
                    selectedFiles,
                    schoolName => new([
                        new("Download data", $"/my-school/download-data/"),
                        new("Data files available for download", $"/my-school/download-data/select-files/?selectedYear={selectedYear}")
                    ], $"Download {schoolName ?? "school"} data"),
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

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName, LocalAuthorityDTO? localAuthority) => 
            new([], "My school");

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string urn, string schoolName) => [
            new("My school", $"/my-school/"),
        ];

        protected override NavigationViewModel GetSubNavigation(
            EstablishmentDetailsViewModel establishmentDetails, 
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("download-data", "Download data", $"/my-school/download-data/", requestPath),
                new("other-reports", "Other reports", $"/my-school/other-reports/", requestPath),
                new("useful-links", "Useful links", $"/my-school/useful-links/", requestPath)
            ]));
        }

        protected override NavigationViewModel GetSideNavigation(
            EstablishmentDetailsViewModel establishmentDetails, 
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("name", $"{establishmentDetails.Name ?? "school"} data", $"/my-school/download-data/", requestPath)
            ]));
        }

        protected override Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string laCode)
        {
            return base.GetEstablishmentDetails(laCode)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        protected override Task<Result<SchoolPageViewModel>> GetSchoolPage(
            EstablishmentDetailsViewModel establishmentDetails,
            BreadcrumbTrailViewModel breadcrumb,
            NavigationViewModel? subNavigation,
            NavigationViewModel? sideNavigation)
        {
            var subTitle = $"{establishmentDetails.Name} <span>(URN: {establishmentDetails.Urn})</span>";
            var schoolPage = new SchoolPageViewModel(
                "MySchool",
                "My school",
                subTitle,
                establishmentDetails.Name,
                establishmentDetails.Urn,
                breadcrumb,
                subNavigation,
                sideNavigation
            );

            return Task.FromResult(Result.Success(schoolPage));
        }

        protected override Task<Result<BaseDownloadDataModel>> GetDownloadDataPage(
            EstablishmentDetailsViewModel establishmentDetails,
            BreadcrumbTrailViewModel breadcrumb,
            string currentActionName,
            NavigationViewModel? subNavigation = null,
            NavigationViewModel? sideNavigation = null)
        {
            var title = "Download data";
            var subtitle =
                $@"{establishmentDetails.Name} <span class=""govuk-!-font-weight-regular"">(URN: {establishmentDetails.Urn})</span>";

            var (contentTitle, contentTitleCaption) = currentActionName switch
            {
                nameof(DownloadDataSelectYear) => (
                    "Dates available for download",
                    $"{establishmentDetails.Name} data"
                ),
                nameof(DownloadDataSelectFiles) => (
                    "Data files available for download",
                    $"{establishmentDetails.Name} data"
                ),
                nameof(DownloadDataSelectFormat) => (
                    $"Download {establishmentDetails.Name} data",
                    $"{establishmentDetails.Name} data"
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
                "MySchool",
                ""
            );
            return Task.FromResult(Result.Success(downloadDataPage));
        }
    }
}