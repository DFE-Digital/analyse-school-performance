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
using ASP.Core.DataDownloads;

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
            IDataDownloadsScopeValidator scopeValidator,
            IHostEnvironment hostEnvironment
        ) : base(api, scopeValidator, hostEnvironment)
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
                select new ContentPageViewModel(
                    page,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        [Authorize(Policy = Policy.NamedData)]
        public IActionResult DownloadData()
        {
            return RedirectToActionPermanent(nameof(DownloadLocalAuthorityData));
        }

        [HttpGet($"download-data/pupil-level-aggregated-la-data/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/pupil-level-aggregated-la-data/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadLocalAuthorityData(DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataController(DataDownloadsScopeType.LA, ControllerContext, Url, _api, _scopeValidator);

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData)))),
                    stepModel => View(new DownloadDataPageViewModel(
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
                    new() { [DownloadDataStepType.SelectFormat] = new() { Title = "Download pupil level and aggregated LA data" } }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data")]
        [HttpPost($"download-data/individual-school-data")]
        [Authorize(Policy = Policy.NamedData)]
        public async Task<IActionResult> DownloadSchoolDataSearch()
        {
            if (Request.Method == HttpMethods.Post)
            {
                return RedirectToAction(nameof(DownloadSchoolDataSearchResults));
            }

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                select new DownloadSchoolDataSearchPageViewModel(
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail()
                                .Append(new("Download data", Action(nameof(DownloadData)))),
                            "Search for a school"
                        ),
                        "Download data",
                        "Individual school data",
                        GetSubNavigation(),
                        GetDownloadDataSideNavigation(),
                        "Search for a school",
                        "Individual school data"
                    ),
                    "searchTerm"
                );

            return await result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/search-results")]
        [HttpPost($"download-data/individual-school-data/search-results")]
        [Authorize(Policy = Policy.NamedData)]
        public async Task<IActionResult> DownloadSchoolDataSearchResults()
        {
            if (Request.Method == HttpMethods.Post)
            {
                return RedirectToAction(nameof(DownloadSchoolData), new { step = "select-year" });
            }

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                select new DownloadSchoolDataSearchResultsPageViewModel(
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail()
                                .Append(new("Download data", Action(nameof(DownloadData))))
                                .Append(new("Search for a school", Action(nameof(DownloadSchoolDataSearch)))),
                            "Search results for \"{searchTerm}\""
                        ),
                        "Download data",
                        "Individual school data",
                        GetSubNavigation(),
                        GetDownloadDataSideNavigation(),
                        "Search results for \"{searchTerm}\"",
                        "Individual school data"
                    ),
                    "searchTerm"
                );

            return await result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadSchoolData(DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataController(DataDownloadsScopeType.LA, ControllerContext, Url, _api, _scopeValidator);

            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    laCode,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData))))
                        .Append(new("Search for a school", Action(nameof(DownloadSchoolDataSearch))))
                        .Append(new("Search results for \"{searchTerm}\"", Action(nameof(DownloadSchoolDataSearch)))),
                    stepModel => View(new DownloadDataPageViewModel(
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
                    Action(nameof(DownloadLocalAuthorityData), DownloadDataController.InitialRouteValues),
                    Request.Path),
                new("Individual school data",
                    Action(nameof(DownloadSchoolDataSearch), DownloadDataController.InitialRouteValues),
                    Request.Path)
            ]);
    }
}