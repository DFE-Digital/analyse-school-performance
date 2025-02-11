using ASP.Api.Client;
using ASP.Api.Client.DataDownloads;
using ASP.Api.Client.Establishments;
using ASP.Core.Results;
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
    [Authorize(Policy = Policy.AccessToMyLocalAuthority)]
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class MyLocalAuthorityController : BaseLocalAuthorityController
    {
        private readonly DownloadDataController _localAuthorityDataDownloadController;
        private readonly DownloadDataController _individualSchoolDataDownloadController;
        private readonly SchoolSearchController _schoolSearchController;

        public MyLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IOptions<SearchOptions> searchOptions
        ) : base(api, hostEnvironment)
        {
            _localAuthorityDataDownloadController = new DownloadDataController(
                nameof(DownloadLocalAuthorityData),
                "MyLocalAuthority",
                [],
                DataDownloadsScopeType.LA,
                _api);

            _individualSchoolDataDownloadController = new DownloadDataController(
                nameof(DownloadSchoolData), 
                "MyLocalAuthority", 
                SearchParameters.RouteValueKeys, 
                DataDownloadsScopeType.School, 
                _api,
                stepRouteConfig: new()
                {
                    [DownloadDataSubActionType.SelectYear] = new() { Path = "select-year" },
                });

            _schoolSearchController = new SchoolSearchController(
                nameof(DownloadSchoolDataSearch),
                "MyLocalAuthority",
                [],
                _api,
                searchOptions.Value,
                makeSchoolUrl: urn => _individualSchoolDataDownloadController.GetInitialActionUrl(new { urn }));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _localAuthorityDataDownloadController.BindContext(context);
            _individualSchoolDataDownloadController.BindContext(context);
            _schoolSearchController.BindContext(context);
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let page = new PageViewModel(
                    new BreadcrumbTrailViewModel(GetBaseBreadcrumbTrail()),
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
        public Task<IActionResult> DownloadLocalAuthorityData(DownloadDataParameters parameters)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from actionResult in _localAuthorityDataDownloadController.Handle(
                    laCode,
                    parameters,
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
                    new() { [DownloadDataSubActionType.SelectFormat] = new() { Title = "Download pupil level and aggregated LA data" } }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{SchoolSearchController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{SchoolSearchController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public async Task<IActionResult> DownloadSchoolDataSearch(SearchParameters parameters)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from action in _schoolSearchController.Handle(
                    new EstablishmentScopeInfo(EstablishmentScopeType.LA, laCode),
                    parameters,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData)))),
                    model => View(new DownloadSchoolDataSearchPageViewModel(
                        new PageViewModel(
                            model.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(),
                            GetDownloadDataSideNavigation(),
                            model.PageTitle,
                            "Individual school data"
                        ),
                        model.Search,
                        model.SearchResults
                    )),
                    new() {
                        [SearchSubActionType.AllListings] = new() {
                            Title = "Search for a school",
                        }
                    })
                select action;

            return await result
                .ToActionResult(_hostEnvironment);
        }

        [HttpGet($"download-data/individual-school-data/{{urn:int:length(6)}}/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/individual-school-data/{{urn:int:length(6)}}/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadSchoolData(string urn, DownloadDataParameters parameters, string? search)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from laName in GetLocalAuthorityName(laCode)
                from schoolName in GetEstablishmentName(urn)
                let baseBreadcrumbTrail = GetChildPageBaseBreadcrumbTrail()
                    .Append(new("Download data", Action(nameof(DownloadData))))
                    .Append(new("Search for a school", Action(nameof(DownloadSchoolDataSearch))))
                let breadcrumbTrail = string.IsNullOrEmpty(search)
                    ? baseBreadcrumbTrail
                    : baseBreadcrumbTrail
                        .Append(new($"Search results for \"{search}\"", Action(nameof(DownloadSchoolDataSearch), new { search })))
                from actionResult in _individualSchoolDataDownloadController.Handle(
                    urn,
                    parameters,
                    breadcrumbTrail,
                    stepModel => View(new DownloadDataPageViewModel(
                        new PageViewModel(
                            stepModel.BreadcrumbTrail,
                            "Download data",
                            "Individual school data",
                            GetSubNavigation(),
                            GetDownloadDataSideNavigation(),
                            stepModel.StepTitle,
                            $"{schoolName} (URN: {urn})"
                        ),
                        stepModel.DownloadData
                    )),
                    new() {
                        [DownloadDataSubActionType.SelectFormat] = new() { Title = "Download individual school data" }
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
                new("Pupil level and aggregated LA data", _localAuthorityDataDownloadController.GetInitialActionUrl(),
                    Request.Path),
                new("Individual school data", _schoolSearchController.GetInitialActionUrl(),
                    Request.Path)
            ]);
    }
}