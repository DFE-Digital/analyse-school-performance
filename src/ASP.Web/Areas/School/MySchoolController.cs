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
    [Route("my-school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchool)]
    public class MySchoolController : BaseSchoolController
    {
        private readonly DownloadDataController _downloadDataController;

        public MySchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
            _downloadDataController = new DownloadDataController(
                nameof(DownloadData),
                "MySchool",
                [],
                DownloadsScopeType.School,
                _api);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _downloadDataController.BindContext(context);
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from urn in User.GetSchoolUrn()
                from schoolDetails in GetSchoolDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetBaseBreadcrumbTrail()),
                        "My school",
                        schoolDetails.Name
                ))
                from linkedSchools in GetLinkedSchools(urn, 
                    urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { urn }))
                select new SchoolLandingPageViewModel(
                    schoolPage,
                    schoolDetails,
                    contentTemplate,
                    linkedSchools
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string? revision)
        {
            var result =
                from urn in User.GetSchoolUrn()
                from schoolDetails in GetSchoolDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail()),
                        "Other reports",
                        schoolDetails.Name,
                        GetSubNavigation()
                ))
                select new SchoolContentPageViewModel(
                    schoolPage,
                    contentTemplate
                );

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string? revision)
        {
            var result =
                from urn in User.GetSchoolUrn()
                from schoolDetails in GetSchoolDetails(urn)
                from contentTemplate in GetContentTemplate(USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail()),
                        "Useful links",
                        schoolDetails.Name,
                        GetSubNavigation()
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
        public Task<IActionResult> DownloadData(DownloadDataParameters parameters)
        {
            var result =
                from urn in User.GetSchoolUrn()
                from schoolDetails in GetSchoolDetails(urn)
                from actionResult in _downloadDataController.Handle(
                    urn,
                    parameters,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", _downloadDataController.GetInitialActionUrl())),
                    stepModel => View(new SchoolDownloadDataPageViewModel(
                        new SchoolPageViewModel(
                            urn,
                            new PageViewModel(
                                stepModel.BreadcrumbTrail,
                                "Download data",
                                $"{schoolDetails.Name} (URN: {urn})",
                                GetSubNavigation(),
                                GetDownloadDataSideNavigation(schoolDetails.Name),
                                stepModel.StepTitle,
                                $"{schoolDetails.Name} data"
                        )),
                        stepModel.DownloadData
                    )),
                    new() { [DownloadDataSubActionType.SelectFormat] = new() { Title = $"Download {schoolDetails.Name} data" } }
                )
                select actionResult;

            return result
                .ToActionResult(_hostEnvironment);
        }

        protected override Task<Result<SchoolDetailsViewModel>> GetSchoolDetails(string urn)
        {
            return base.GetSchoolDetails(urn)
                .MapErrorIf(e => e is NotFoundError, m => Error.Unexpected(m, null));
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() => [];
        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail() =>
            GetBaseBreadcrumbTrail().Concat([
                new("My school", Action(nameof(LandingPage))),
            ]);

        private NavigationViewModel GetSubNavigation()
        {
            List<NavigationItemViewModel> navigationItems = new();

            if (User.HasRole(Role.NamedData))
            {
                navigationItems.Add(new("Download data", _downloadDataController.GetInitialActionUrl(), Request.Path));
            }

            navigationItems.AddRange(
            [
                new NavigationItemViewModel("Other reports", Action(nameof(OtherReports)), Request.Path),
                new NavigationItemViewModel("Useful links", Action(nameof(UsefulLinks)), Request.Path)
            ]);

            return new NavigationViewModel(navigationItems);
        }

        private NavigationViewModel GetDownloadDataSideNavigation(string name) =>
            new([
                new($"{name} data", _downloadDataController.GetInitialActionUrl(), Request.Path)
            ]);
    }
}