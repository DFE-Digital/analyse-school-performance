using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Features.DataDownloads;
using ASP.Core.DataDownloads;

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
            IDataDownloadsScopeValidator scopeValidator,
            IHostEnvironment hostEnvironment)
            : base(api, scopeValidator, hostEnvironment)
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
                select new ContentPageViewModel(
                    page,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        [Authorize(Policy = Policy.NamedData)]
        public IActionResult DownloadData(string laCode)
        {
            return RedirectToActionPermanent(nameof(DownloadLocalAuthorityData), new { laCode });
        }

        [HttpGet($"download-data/pupil-level-aggregated-la-data/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/pupil-level-aggregated-la-data/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadLocalAuthorityData(string laCode, DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataController(DataDownloadsScopeType.LA, ControllerContext, Url, _api, _scopeValidator);

            var result =
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName)
                        .Append(new("Download data", Action(nameof(DownloadData), new { laCode }))),
                    stepModel => View(new DownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Pupil level and aggregated LA data",
                            GetSubNavigation(laCode),
                            GetDownloadDataSideNavigation(laCode),
                            stepModel.StepTitle,
                            "Pupil level and aggregated LA data"
                        ),
                        stepModel.DownloadData
                    )),
                    new()
                    {
                        [DownloadDataStepType.SelectFormat] = new() { Title = "Download pupil level and aggregated LA data" }
                    }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadSchoolData(string laCode, DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataController(DataDownloadsScopeType.LA, ControllerContext, Url, _api, _scopeValidator);

            var result =
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName)
                        .Append(new("Download data", Action(nameof(DownloadData), new { laCode }))),
                    stepModel => View(new DownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(laCode),
                            GetDownloadDataSideNavigation(laCode),
                            stepModel.StepTitle,
                            "Individual school data"
                        ),
                        stepModel.DownloadData
                    )),
                    new()
                    {
                        [DownloadDataStepType.SelectYear] = new() { Path = "select-year/" },
                        [DownloadDataStepType.SelectFormat] = new() { Title = "Download individual school data" }
                    }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
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
                new("Download data", Action(nameof(DownloadData),
                    new { laCode }), Request.Path),
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string laCode) =>
            new([
                new("Pupil level and aggregated LA data", Action(nameof(DownloadLocalAuthorityData),
                    DownloadDataController.InitialRouteValues.Merge(new { laCode })), Request.Path),
                new("Individual school data", Action(nameof(DownloadSchoolData),
                    DownloadDataController.InitialRouteValues.Merge(new { laCode })), Request.Path)
            ]);
    }
}