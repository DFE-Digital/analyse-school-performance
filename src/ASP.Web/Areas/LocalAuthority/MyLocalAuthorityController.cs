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
        public async Task<IActionResult> LandingPage(string? revision)
        {
            return await User.GetLocalAuthorityCode()
                .Then(laCode => base.LandingPage(laCode, revision))
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public async Task<IActionResult> DownloadData()
        {
            return await User.GetLocalAuthorityCode()
                .Then(laCode => base.DownloadData(laCode))
                .ToActionResult(View, _hostEnvironment);
        }

        protected override Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return base.GetLocalAuthorityName(laCode)
                .MapError(error => error is NotFoundError 
                    ? Error.Unexpected(error.Message, null) 
                    : error);
        }

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName, string? page = null)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                "My local authority",
                laName,
                page == null
                    ? new BreadcrumbTrailViewModel("My local authority")
                    : new BreadcrumbTrailViewModel(page)
                        .AddBreadcrumb("My local authority", $"/my-local-authority/")
            );

            return Task.FromResult(Result.Success(viewModel));
        }
    }
}