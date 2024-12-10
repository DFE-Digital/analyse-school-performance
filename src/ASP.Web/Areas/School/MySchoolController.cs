using ASP.Application;
using ASP.Core.Authorization;
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

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchool)]
    public class MySchoolController : BaseSchoolController
    {
        public MySchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetBaseBreadcrumbTrail(), "My school"),
                        "My school",
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
        public Task<IActionResult> OtherReports(string? revision)
        {
            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail(), "Other reports"),
                        "Other reports",
                        establishmentDetails.Name,
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
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from contentTemplate in GetContentTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision)
                let schoolPage = new SchoolPageViewModel(
                    urn,
                    new PageViewModel(
                        new BreadcrumbTrailViewModel(GetChildPageBaseBreadcrumbTrail(), "Useful links"),
                        "Useful links",
                        establishmentDetails.Name,
                        GetSubNavigation()
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
        public Task<IActionResult> DownloadData(DownloadDataStepParameters parameters)
        {
            var downloadData = new DownloadDataStepController(DownloadDataScope.School, ControllerContext, Url, _api);

            var result =
                from urn in User.GetEstablishmentUrn()
                from establishmentDetails in GetEstablishmentDetails(urn)
                from actionResult in downloadData.HandleStep(
                    parameters,
                    urn,
                    GetChildPageBaseBreadcrumbTrail()
                        .Append(new("Download data", Action(nameof(DownloadData), 
                            DownloadDataStepController.InitialRouteValues))),
                    stepModel => View(new SchoolDownloadDataPageViewModel(
                        new SchoolPageViewModel(
                            urn,
                            new PageViewModel(
                                stepModel.BreadcrumbTrail,
                                "Download data",
                                establishmentDetails.Name,
                                GetSubNavigation(),
                                GetDownloadDataSideNavigation(establishmentDetails.Name),
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

        protected override Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string urn)
        {
            return base.GetEstablishmentDetails(urn)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        private IEnumerable<BreadcrumbItem> GetBaseBreadcrumbTrail() => [];
        private IEnumerable<BreadcrumbItem> GetChildPageBaseBreadcrumbTrail() => 
            GetBaseBreadcrumbTrail().Concat([
                new("My school", Action(nameof(LandingPage))),
            ]);

        private NavigationViewModel GetSubNavigation() =>
            new([
                new("Download data", Action(nameof(DownloadData), 
                    DownloadDataStepController.InitialRouteValues), Request.Path),
                new("Other reports", Action(nameof(OtherReports)), Request.Path),
                new("Useful links", Action(nameof(UsefulLinks)), Request.Path)
            ]);

        private NavigationViewModel GetDownloadDataSideNavigation(string name) =>
            new([
                new($"{name} data", Action(nameof(DownloadData),
                    DownloadDataStepController.InitialRouteValues), Request.Path)
            ]);
    }
}