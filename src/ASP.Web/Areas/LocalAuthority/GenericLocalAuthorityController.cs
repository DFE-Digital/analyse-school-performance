using ASP.Application;
using ASP.Core.Results;
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
    [Authorize(Policy = Policy.AccessToAllLocalAuthorities)]
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class GenericLocalAuthorityController : LocalAuthorityController
    {
        public GenericLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment)
            : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public new Task<IActionResult> LandingPage(string laCode, string? revision)
        {
            return base.LandingPage(laCode, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName) => new([
            new("All local authorities", $"/local-authorities/")
        ], laName);

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName) =>
        [
            new("All local authorities", $"/local-authorities/"),
            new(laName, $"/local-authority/{laCode}/"),
        ];

        protected override NavigationViewModel GetSubNavigation(
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("download-data", "Download data", $"/local-authority/download-data/", requestPath)
            ]));
        }

        protected override NavigationViewModel GetSideNavigation(
            PathString requestPath)
        {
            return new NavigationViewModel(new([
                new("pupil-level-and-aggregate-la-data", $"Pupil level and aggregate LA data",
                    $"/local-authority/download-data/", requestPath),
                new("individual-school-data", $"Individual school data", $"/local-authority/individual-school-data/", requestPath)
            ]));
        }

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName,
            BreadcrumbTrailViewModel breadcrumbs)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                laName,
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
                "GenericLocalAuthority",
                ""
            );
            return Task.FromResult(Result.Success(downloadDataPage));
        }
    }
}