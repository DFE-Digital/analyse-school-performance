using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToMyLocalAuthority)]
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class MyLocalAuthorityController : LocalAuthorityController
    {
        public MyLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> LandingPage(string? revision)
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.LandingPage(laCode, revision)
                select model;

            return result
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]

        public Task<IActionResult> DownloadData()
        {
            var result =
                from laCode in User.GetLocalAuthorityCode()
                from model in base.DownloadData(laCode, new(
                    [], 
                    "Download data"))
                select model;

            return result
                .ToActionResult(View, _hostEnvironment);
        }
		
        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName) => new(
            [], 
            "My local authority");

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName) => [
            new("My local authority", $"/my-local-authority/")
        ];

        protected override Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return base.GetLocalAuthorityName(laCode)
                .MapError(error => error is NotFoundError
                    ? Error.Unexpected(error.Message, null)
                    : error);
        }

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName, BreadcrumbTrailViewModel breadcrumbs)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                "My local authority",
                laName,
                breadcrumbs
            );

            return Task.FromResult(Result.Success(viewModel));
        }
    }
}