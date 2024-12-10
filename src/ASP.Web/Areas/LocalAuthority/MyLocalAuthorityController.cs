using ASP.Application;
using ASP.Core.Authorization;
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
            return RedirectToActionPermanent(nameof(DownloadPupilLevelAggregatedLAData));
        }

        [HttpGet($"download-data/pupil-level-aggregated-la-data/{DownloadDataStepController.SubRouteTemplate}")]
        [HttpPost($"download-data/pupil-level-aggregated-la-data/{DownloadDataStepController.SubRouteTemplate}")]
        public Task<IActionResult> DownloadPupilLevelAggregatedLAData(DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.LocalAuthority, ControllerContext, Url, _api);

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData)))),
                    stepModel => View(new LocalAuthorityDownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Pupil level and aggregated LA data",
                            GetSubNavigation(),
                            GetDownloadDataSideNavigation(),
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
        public Task<IActionResult> DownloadIndividualSchoolData(DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.LocalAuthority, ControllerContext, Url, _api);

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData)))),
                    stepModel => View(new LocalAuthorityDownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(),
                            GetDownloadDataSideNavigation(),
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
                new("Pupil level and aggregated LA data", 
                    Action(nameof(DownloadPupilLevelAggregatedLAData), DownloadDataStepController.InitialRouteValues), 
                    Request.Path),
                new("Individual school data", 
                    Action(nameof(DownloadIndividualSchoolData), DownloadDataStepController.InitialRouteValues), 
                    Request.Path)
            ]);
    }
}