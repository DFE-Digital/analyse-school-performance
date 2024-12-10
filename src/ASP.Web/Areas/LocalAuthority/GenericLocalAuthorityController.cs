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
        public IActionResult DownloadData(string laCode)
        {
            return RedirectToActionPermanent(nameof(DownloadPupilLevelAggregatedLAData), new { laCode });
        }

        [HttpGet($"download-data/pupil-level-aggregated-la-data/{DownloadDataStepController.SubRouteTemplate}")]
        [HttpPost($"download-data/pupil-level-aggregated-la-data/{DownloadDataStepController.SubRouteTemplate}")]
        public Task<IActionResult> DownloadPupilLevelAggregatedLAData(string laCode, DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.LocalAuthority, ControllerContext, Url, _api);

            var result =
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName)
                        .Append(new("Download data", Action(nameof(DownloadData), new { laCode }))),
                    stepModel => View(new LocalAuthorityDownloadDataPageViewModel(
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
                    new() { [DownloadDataStepType.SelectFormat] = "Download pupil level and aggregated LA data" }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{DownloadDataStepController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{DownloadDataStepController.SubRouteTemplate}")]
        public Task<IActionResult> DownloadIndividualSchoolData(string laCode, DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.LocalAuthority, ControllerContext, Url, _api);

            var result =
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName)
                        .Append(new("Download data", Action(nameof(DownloadData), new { laCode }))),
                    stepModel => View(new LocalAuthorityDownloadDataPageViewModel(
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
                    new() { [DownloadDataStepType.SelectFormat] = "Download individual school data" }
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
                new("Pupil level and aggregated LA data", Action(nameof(DownloadPupilLevelAggregatedLAData),
                    DownloadDataStepController.InitialRouteValues.Merge(new { laCode })), Request.Path),
                new("Individual school data", Action(nameof(DownloadIndividualSchoolData),
                    DownloadDataStepController.InitialRouteValues.Merge(new { laCode })), Request.Path)
            ]);
    }
}