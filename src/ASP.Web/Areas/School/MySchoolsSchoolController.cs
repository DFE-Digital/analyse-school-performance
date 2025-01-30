using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Results;
using ASP.Domain.DataDownloads;
using ASP.Domain.Establishments;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.DataDownloads;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using ASP.Web.Shared.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-schools/{urn:regex(\\d{{6}})}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsSchoolController : BaseSchoolController
    {
        private readonly DownloadDataController _downloadDataController;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IEstablishmentScopeValidator _establishmentScopeValidator;

        public MySchoolsSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IEstablishmentRepository establishmentRepository,
            IDataDownloadsScopeValidator dataDownloadsScopeValidator,
            IEstablishmentScopeValidator establishmentScopeValidator) 
            : base(api, hostEnvironment)
        {
            _establishmentRepository = establishmentRepository;
            _establishmentScopeValidator = establishmentScopeValidator;
            _downloadDataController = new DownloadDataController(nameof(DownloadData), "MySchoolsSchool", ["urn"], DataDownloadsScopeType.School, _api, dataDownloadsScopeValidator);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _downloadDataController.BindContext(context);
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string urn, string? revision)
        {
            var result =
                from scopeInfo in _establishmentScopeValidator.GetScopeInfoForRole(User)
                from establishmentDetails in GetEstablishmentDetails(urn, scopeInfo)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail()
                        ),
                        "My schools",
                        establishmentDetails.Name
                ))
                select new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string urn, string? revision)
        {
            var result = 
                from scopeInfo in _establishmentScopeValidator.GetScopeInfoForRole(User)
                from establishmentDetails in GetEstablishmentDetails(urn, scopeInfo)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name)
                        ),
                        "Other reports",
                        establishmentDetails.Name,
                        GetSubNavigation(urn)
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            var result =
                from scopeInfo in _establishmentScopeValidator.GetScopeInfoForRole(User)
                from establishmentDetails in GetEstablishmentDetails(urn, scopeInfo)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name)
                        ),
                        "Useful links",
                        establishmentDetails.Name,
                        GetSubNavigation(urn)
                    ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet($"download-data/{DownloadDataController.SubRouteTemplate}")]
        [HttpPost($"download-data/{DownloadDataController.SubRouteTemplate}")]
        [Authorize(Policy = Policy.NamedData)]
        public Task<IActionResult> DownloadData(string urn, DownloadDataStepParameters parameters)
        {
            var result =
                from scopeInfo in _establishmentScopeValidator.GetScopeInfoForRole(User)
                from establishmentDetails in GetEstablishmentDetails(urn, scopeInfo)
                from actionResult in _downloadDataController.Handle(
                    parameters,
                    urn,
                    GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name)
                        .Append(new("Download data", _downloadDataController.GetInitialActionUrl())),
                    stepModel => View(new SchoolDownloadDataPageViewModel(
                        new SchoolPageViewModel(
                            urn,
                            new PageViewModel(
                                stepModel.BreadcrumbTrail,
                                "Download data",
                                $"{establishmentDetails.Name} (URN: {urn})",
                                GetSubNavigation(urn),
                                GetDownloadDataSideNavigation(urn, establishmentDetails.Name),
                                stepModel.StepTitle,
                                $"{establishmentDetails.Name} data"
                        )),
                        stepModel.DownloadData
                    )),
                    new() { [DownloadDataStepType.SelectFormat] = new() { Title = $"Download {establishmentDetails.Name} data" } }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() =>
            [
                new("My schools", "/my-schools/")
            ];

        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail(string urn, string name) =>
            GetBaseBreadcrumbTrail().Concat([
                new(name, Action(nameof(LandingPage),
                    new { urn })),
            ]);

        private NavigationViewModel GetSubNavigation(string urn)
        {
            List<NavigationItemViewModel> navigationItems = new();

            if (User.HasRole(Role.NamedData))
            {
                navigationItems.Add(new("Download data", _downloadDataController.GetInitialActionUrl(), Request.Path));
            }

            navigationItems.AddRange(
            [
                new("Other reports", Action(nameof(OtherReports), new { urn }), Request.Path),
                new("Useful links", Action(nameof(UsefulLinks), new { urn }), Request.Path)
            ]);

            return new NavigationViewModel(navigationItems);
        }

        private NavigationViewModel GetDownloadDataSideNavigation(string urn, string name) =>
            new([
                new($"{name} data", _downloadDataController.GetInitialActionUrl(), Request.Path)
            ]);

        private Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn, EstablishmentScopeInfo scopeInfo)
        {
            return
                from isVisible in _establishmentRepository.IsEstablishmentVisibleWithinScope(urn,
                        new EstablishmentScope(scopeInfo.ScopeType, scopeInfo.ScopeId.GetValueOrDefault("")))
                    .ErrorIf(exists => !exists, Error.NotAllowed($"User is not allowed to view School {urn}"))
                from establishmentDetails in GetEstablishmentDetails(urn)
                select establishmentDetails;
        }
    }
}