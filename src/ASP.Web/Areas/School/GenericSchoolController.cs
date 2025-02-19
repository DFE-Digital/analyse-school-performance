using ASP.Api.Client;
using ASP.Api.Client.Downloads;
using ASP.Core.Authorization;
using ASP.Core.Results;
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
    [Route("school/{urn:int:length(6)}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToGenericSchool)]
    public class GenericSchoolController : BaseSchoolController
    {
        private readonly DownloadDataController _downloadDataController;

        public GenericSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
            _downloadDataController = new DownloadDataController(
                nameof(DownloadData),
                "GenericSchool",
                ["urn"],
                DownloadsScopeType.School,
                _api);
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
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail(laCode, laName)
                        ),
                        establishmentDetails.Name
                ))
                from linkedEstablishments in GetLinkedSchools(urn, 
                    urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { urn }))
                select new SchoolLandingPageViewModel(
                    schoolPage,
                    establishmentDetails,
                    contentTemplate,
                    linkedEstablishments
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string urn, string? revision)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name)
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
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name)
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
        public Task<IActionResult> DownloadData(string urn, DownloadDataParameters parameters)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                from actionResult in _downloadDataController.Handle(
                    urn,
                    parameters,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name)
                        .Append(new("Download data", _downloadDataController.GetInitialActionUrl(new { urn }))),
                    stepModel => View(new SchoolDownloadDataPageViewModel(
                        new SchoolPageViewModel(
                            urn,
                            new PageViewModel(
                                stepModel.BreadcrumbTrail,
                                "Download data",
                                $"{establishmentDetails.Name} (URN: {urn})",
                                GetSubNavigation(urn),
                                GetDownloadDataSideNavigation(establishmentDetails.Name),
                                stepModel.StepTitle,
                                $"{establishmentDetails.Name} data"
                        )),
                        stepModel.DownloadData
                    )),
                    new() { [DownloadDataSubActionType.SelectFormat] = new() { Title = $"Download {establishmentDetails.Name} data" } }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail(string laCode, string laName)
        {
            if (User.HasRole(Role.AccessToMySchool))
            {
                return [];
            }
            
            return
            [
                new("All local authorities", $"/local-authorities/"),
                new(laName, $"/local-authority/{laCode}/"),
                new("All schools", $"/local-authority/{laCode}/schools/")
            ];
        }

        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail(string laCode, string laName, string urn, string name) =>
            GetBaseBreadcrumbTrail(laCode, laName).Concat([
                new(name, Action(nameof(LandingPage), new { urn })),
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

        private NavigationViewModel GetDownloadDataSideNavigation(string name) =>
            new([
                new($"{name} data", _downloadDataController.GetInitialActionUrl(), Request.Path)
            ]);
        
        protected override Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return
                from scopeInfo in User.GetScopeInfoForRole()
                from _ in _api.SchoolsGetAccess(new(urn, scopeInfo))
                    .ErrorIf(response => !(response.IsAccessibleInScope || response.IsAccessibleViaLinkedSchools), 
                        Error.NotAllowed($"User is not allowed to view School {urn}"))
                from establishmentDetails in base.GetEstablishmentDetails(urn)
                select establishmentDetails;
        }
    }
}
