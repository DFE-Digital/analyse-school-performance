using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Features.DataDownloads;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-schools/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsSchoolController : BaseSchoolController
    {
        public MySchoolsSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string urn, string? revision)
        {
            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetBaseBreadcrumbTrail(), 
                            establishmentDetails.Name
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
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name), 
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
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(
                            GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name), 
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

        [HttpGet($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
        [HttpPost($"download-data/{DownloadDataStepController.SubRouteTemplate}")]
        public Task<IActionResult> DownloadData(string urn, DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.School, ControllerContext, Url, _api);

            var result =
                from establishmentDetails in GetEstablishmentDetails(urn)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    urn,
                    GetChildPageBaseBreadcrumbTrail(urn, establishmentDetails.Name)
                        .Append(new("Download data", Action(nameof(DownloadData),
                            DownloadDataStepController.InitialRouteValues.Merge(new { urn })))),
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
                    new() { [DownloadDataStepType.SelectFormat] = $"Download {establishmentDetails.Name} data" }
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

        private NavigationViewModel GetSubNavigation(string urn) =>
            new([
                new("Download data", Action(nameof(DownloadData),
                    DownloadDataStepController.InitialRouteValues.Merge(new { urn })), Request.Path),
                new("Other reports", Action(nameof(OtherReports), 
                    new { urn }), Request.Path),
                new("Useful links", Action(nameof(UsefulLinks),
                    new { urn }), Request.Path)
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string urn, string name) =>
            new([
                new($"{name} data", Action(nameof(DownloadData),
                    DownloadDataStepController.InitialRouteValues.Merge(new { urn })), Request.Path)
            ]);
    }
}