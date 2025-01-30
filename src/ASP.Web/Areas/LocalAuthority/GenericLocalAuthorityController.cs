using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.DataDownloads;
using ASP.Domain.Establishments;
using ASP.Web.Areas.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.DataDownloads;
using ASP.Web.Features.Search;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using ASP.Web.Shared.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToAllLocalAuthorities)]
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode:int:length(3)}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class GenericLocalAuthorityController : BaseLocalAuthorityController
    {
        private readonly DownloadDataController _localAuthorityDataDownloadController;
        private readonly DownloadDataController _individualSchoolDataDownloadController;
        private readonly SchoolSearchController _schoolSearchController;

        public GenericLocalAuthorityController(
            IAspApiClient api,
            IDataDownloadsScopeValidator scopeValidator,
            IHostEnvironment hostEnvironment,
            IOptions<SearchOptions> searchOptions)
            : base(api, scopeValidator, hostEnvironment)
        {
            _localAuthorityDataDownloadController = new DownloadDataController(nameof(DownloadLocalAuthorityData), "GenericLocalAuthority", ["laCode"], DataDownloadsScopeType.LA, _api, _scopeValidator);

            _individualSchoolDataDownloadController = new DownloadDataController(nameof(DownloadSchoolData), "GenericLocalAuthority", ["laCode", .. SearchParameters.RouteValueKeys], DataDownloadsScopeType.School, _api, _scopeValidator,
                stepRouteConfig: new()
                {
                    [DownloadDataStepType.SelectYear] = new() { Path = "select-year" },
                });

            _schoolSearchController = new SchoolSearchController(nameof(DownloadSchoolDataSearch), "GenericLocalAuthority", ["laCode"], _api,
                searchOptions.Value, makeSchoolUrl: urn => _individualSchoolDataDownloadController.GetInitialActionUrl(new { urn }));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _localAuthorityDataDownloadController.BindContext(context);
            _individualSchoolDataDownloadController.BindContext(context);
            _schoolSearchController.BindContext(context);
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string laCode, string? revision)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(GetBaseBreadcrumbTrail()),
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
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in _localAuthorityDataDownloadController.Handle(
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

        [HttpGet($"download-data/individual-school-data/{SchoolSearchController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{SchoolSearchController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public async Task<IActionResult> DownloadSchoolDataSearch(string laCode, SearchParameters parameters)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from action in _schoolSearchController.Handle(
                    new EstablishmentScopeInfo(EstablishmentScopeType.LA, Optional<string>.Some(laCode)),
                    parameters,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName)
                        .Append(new("Download data", Action(nameof(DownloadData), new { laCode }))),
                    model => View(new DownloadSchoolDataSearchPageViewModel(
                        new PageViewModel(
                            model.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(laCode),
                            GetDownloadDataSideNavigation(laCode),
                            model.PageTitle,
                            "Individual school data"
                        ),
                        model.Search,
                        model.Establishments
                    )),
                    new()
                    {
                        [SchoolSearchSubActionType.AllSchools] = new()
                        {
                            Title = "Search for a school",
                        }
                    })
                select action;

            return await result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{{urn:int:length(6)}}/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{{urn:int:length(6)}}/{DownloadDataController.SubRouteTemplate}")]
        public Task<IActionResult> DownloadSchoolData(string laCode, string urn, DownloadDataStepParameters parameters, string? search)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from schoolName in GetEstablishmentName(urn)
                let baseBreadcrumbTrail = GetChildPageBaseBreadcrumbTrail(laCode, laName)
                   .Append(new("Download data", Action(nameof(DownloadData), new { laCode })))
                   .Append(new("Search for a school", Action(nameof(DownloadSchoolDataSearch), new { laCode })))
                let breadcrumbTrail = string.IsNullOrEmpty(search)
                    ? baseBreadcrumbTrail
                    : baseBreadcrumbTrail
                        .Append(new($"Search results for \"{search}\"", Action(nameof(DownloadSchoolDataSearch), new { laCode, search })))
                from actionResult in _individualSchoolDataDownloadController.Handle(
                    parameters,
                    urn,
                    breadcrumbTrail,
                    stepModel => View(new DownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(laCode),
                            GetDownloadDataSideNavigation(laCode),
                            stepModel.StepTitle,
                            $"{schoolName} (URN: {urn})"
                        ),
                        stepModel.DownloadData
                    )),
                    new()
                    {
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
                new("Pupil level and aggregated LA data", _localAuthorityDataDownloadController.GetInitialActionUrl(),
                    Request.Path),
                new("Individual school data", _schoolSearchController.GetInitialActionUrl(),
                    Request.Path)
             ]);

    }
}