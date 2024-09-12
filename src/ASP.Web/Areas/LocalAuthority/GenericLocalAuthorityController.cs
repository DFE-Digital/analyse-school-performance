using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToAllLocalAuthorities)]
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class GenericLocalAuthorityController : LocalAuthorityController
    {
        public GenericLocalAuthorityController(
            IAspApiClient api, 
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public new Task<IActionResult> LandingPage(string laCode, string? revision)
        {
            return base.LandingPage(laCode, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public new Task<IActionResult> DownloadData(string laCode)
        {
            return base.DownloadData(laCode)
                .ToActionResult(View, _hostEnvironment);
        }

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName, string? page = null)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                laName,
                laName,
                page == null
                    ? new BreadcrumbTrailViewModel(laName)
                    : new BreadcrumbTrailViewModel(page)
                        .AddBreadcrumb(laName, $"local-authority/{laCode}")
            );

            return Task.FromResult(Result.Success(viewModel));
        }
    }
}