using ASP.Application;
using ASP.Core.Authorization;
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
    [Route("my-school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchool)]
    public class MySchoolController : SchoolController
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
            return User.GetEstablishmentUrn()
                .Then(urn => base.LandingPage(urn, revision))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("other-reports")]
        public Task<IActionResult> OtherReports(string? revision)
        {
            return User.GetEstablishmentUrn()
                .Then(urn => base.OtherReports(urn, revision))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("useful-links")]
        public Task<IActionResult> UsefulLinks(string? revision)
        {
            return User.GetEstablishmentUrn()
                .Then(urn => base.UsefulLinks(urn, revision))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadData()
        {
            return User.GetEstablishmentUrn()
                .Then(urn => base.DownloadData(urn))
                .ToActionResult(View, _hostEnvironment);
        }

        protected override Task<Result<EstablishmentDetailsViewModel>> GetEstablishmentDetails(string laCode)
        {
            return base.GetEstablishmentDetails(laCode)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        protected override Task<Result<SchoolPageViewModel>> GetSchoolPage(EstablishmentDetailsViewModel establishmentDetails, string? page = null)
        {
            var schoolPage = new SchoolPageViewModel(
                "MySchool",
                "My school",
                establishmentDetails.Name,
                establishmentDetails.Urn,
                Request.Path,
                $"/my-school/",
                page == null
                    ? new BreadcrumbTrailViewModel("My school")
                    : new BreadcrumbTrailViewModel(page)
                        .AddBreadcrumb("My school", $"/my-school/")
            );

            return Task.FromResult(Result.Success(schoolPage));
        }
    }
}
