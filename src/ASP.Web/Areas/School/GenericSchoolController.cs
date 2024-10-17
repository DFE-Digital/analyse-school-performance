using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.Navigation;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolController : SchoolController
    {
        public GenericSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public new Task<IActionResult> LandingPage(string urn, string? revision)
        {
            return base.LandingPage(urn, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string urn, string? revision)
        {
            return base.OtherReports(urn, revision, schoolName => new([], "Other reports"))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            return base.UsefulLinks(urn, revision, schoolName => new([], "Useful links"))
                .ToActionResult(View, _hostEnvironment);
        }

        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string schoolName) => new([
            new($"{schoolName}", $"/my-schools/")], schoolName);

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string urn, string schoolName) => [
                    new("My schools", $"/my-schools/"),
            new(schoolName, $"/school/{urn}"),
        ];

        protected override NavigationViewModel GetSubNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath)
        {
            return new NavigationViewModel(new([
                    new("download-data", "Download data", $"/school/{establishmentDetails.Urn}/download-data/", requestPath),
                    new("other-reports", "Other reports", $"/school/{establishmentDetails.Urn}/other-reports/", requestPath),
                    new("useful-links", "Useful links", $"/school/{establishmentDetails.Urn}/useful-links/", requestPath)
                ]));
        }

        protected override NavigationViewModel GetSideNavigation(EstablishmentDetailsViewModel establishmentDetails, PathString requestPath)
        {
            return new NavigationViewModel(new([new("name", $"{establishmentDetails.Name} data", $"/school/{establishmentDetails.Urn}/download-data/", requestPath)]));
        }

        protected override Task<Result<SchoolPageViewModel>> GetSchoolPage(EstablishmentDetailsViewModel establishmentDetails,
                                                                           BreadcrumbTrailViewModel breadcrumbs,
                                                                           NavigationViewModel? subNavigation,
                                                                           NavigationViewModel? sideNavigation)
        {
            var schoolPage = new SchoolPageViewModel(
                "GenericSchool",
                establishmentDetails.Name,
                establishmentDetails.Name,
                establishmentDetails.Urn,
                breadcrumbs,
                subNavigation,
                sideNavigation
            );

            return Task.FromResult(Result.Success(schoolPage));
        }
    }
}
