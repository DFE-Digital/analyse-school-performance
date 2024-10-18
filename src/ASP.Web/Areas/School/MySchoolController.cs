using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
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
                from model in base.UsefulLinks(urn, revision, schoolName => new([], "Useful links"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadDataSelectYear()
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectYear(urn, schoolName => new([], "Download data"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/select-files")]
        public Task<IActionResult> DownloadDataSelectFiles(int? selectedYear)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from model in base.DownloadDataSelectFiles(
                    urn,
                    Optional.FromNullable(selectedYear),
                    schoolName => new([
                        new("Download data", $"/my-school/download-data/")
                    ], "Data files available for download"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
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
                        new("Data files available for download", $"/my-school/download-data/select-files/?SelectedYear={selectedYear}")
                    ], $"Download {schoolName ?? "school"} data"))
                select model;

            return result.ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data/download-as-zip")]
        public Task<IActionResult> DownloadDataAsZip(FileType fileType, List<string> selectedFiles)
        {
            return base.GetDownloadsAsZipFile(fileType, selectedFiles)
                .ToActionResult(_hostEnvironment);
        }

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName) => 
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
            var schoolPage = new SchoolPageViewModel(
                "MySchool",
                "My school",
                establishmentDetails.Name,
                establishmentDetails.Urn,
                breadcrumb,
                subNavigation,
                sideNavigation
            );

            return Task.FromResult(Result.Success(schoolPage));
        }
    }
}