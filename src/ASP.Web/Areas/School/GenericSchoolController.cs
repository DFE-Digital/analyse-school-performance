using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Features.DataDownloads;
using ASP.Web.Shared;
using ASP.Core.DataDownloads;
using ASP.Core.Authorization;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolController : BaseSchoolController
    {
        public GenericSchoolController(
            IAspApiClient api,
            IDataDownloadsScopeValidator scopeValidator,
            IHostEnvironment hostEnvironment
        ) : base(api, scopeValidator, hostEnvironment)
        {
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
                            GetBaseBreadcrumbTrail(laCode, laName),
                            establishmentDetails.Name
                        ),
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
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name),
                            "Other reports"
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
                            GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name),
                            "Useful links"
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
            var downloadData = new DownloadDataController(DataDownloadsScopeType.School, ControllerContext, Url, _api, _scopeValidator);

            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                let laCode = establishmentDetails.LocalAuthority?.Code ?? ""
                let laName = establishmentDetails.LocalAuthority?.Name ?? ""
                from actionResult in downloadData.HandleStep(
                    parameters,
                    urn,
                    GetChildPageBaseBreadcrumbTrail(laCode, laName, urn, establishmentDetails.Name)
                        .Append(new("Download data", Action(nameof(DownloadData),
                            DownloadDataController.InitialRouteValues.Merge(new { urn })))),
                    stepModel => View(new SchoolDownloadDataPageViewModel(
                        new SchoolPageViewModel(
                            urn,
                            new PageViewModel(
                                stepModel.BreadcrumbTrail,
                                "Download data",
                                establishmentDetails.Name,
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

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail(string laCode, string laName) =>
            [
                new("All local authorities", $"/local-authorities/"),
                new(laName, $"/local-authority/{laCode}/"),
                new("All schools", $"/local-authority/{laCode}/schools/")
            ];

        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail(string laCode, string laName, string urn, string name) =>
            GetBaseBreadcrumbTrail(laCode, laName).Concat([
                new(name, Action(nameof(LandingPage),
                    new { urn })),
            ]);

        private NavigationViewModel GetSubNavigation(string urn)
        {
            List<NavigationItemViewModel> navigationItems = new();

            if (User.HasRole(Role.NamedData))
            {
                navigationItems.Add(new NavigationItemViewModel("Download data", Action(nameof(DownloadData),
                    DownloadDataController.InitialRouteValues.Merge(new { urn })), Request.Path));
            }

            navigationItems.AddRange(
            [
                new NavigationItemViewModel("Other reports", Action(nameof(OtherReports), new { urn }), Request.Path),
                new NavigationItemViewModel("Useful links", Action(nameof(UsefulLinks), new { urn }), Request.Path)
            ]);

            return new NavigationViewModel(navigationItems);
        }

        private NavigationViewModel GetDownloadDataSideNavigation(string urn, string name) =>
            new([
                new($"{name} data", Action(nameof(DownloadData),
                    DownloadDataController.InitialRouteValues.Merge(new { urn })), Request.Path)
            ]);
    }
}
