using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
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
        public new Task<IActionResult> OtherReports(string urn, string? revision)
        {
            return base.OtherReports(urn, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public new Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            return base.UsefulLinks(urn, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public new Task<IActionResult> DownloadData(string urn)
        {
            return base.DownloadData(urn)
                .ToActionResult(View, _hostEnvironment);
        }

        protected override Task<Result<SchoolPageViewModel>> GetSchoolPage(EstablishmentDetailsViewModel establishmentDetails, string? page = null)
        {
            var schoolPage = new SchoolPageViewModel(
                "GenericSchool",
                establishmentDetails.Name,
                establishmentDetails.Name,
                establishmentDetails.Urn,
                Request.Path,
                $"/school/{establishmentDetails.Urn}/",
                page == null
                    ? new BreadcrumbTrailViewModel(establishmentDetails.Name)
                    : new BreadcrumbTrailViewModel(page)
                        .AddBreadcrumb(establishmentDetails.Name, $"/school/{establishmentDetails.Urn}/")
            );

            return Task.FromResult(Result.Success(schoolPage));
        }
    }
}
