using ASP.Application;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
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
            IHostEnvironment hostEnvironment) 
            : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public new Task<IActionResult> LandingPage(string laCode, string? revision)
        {            
            return base.LandingPage(laCode, revision)
                .ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("download-data")]
        public Task<IActionResult> DownloadData(string laCode)
        {
            return base.DownloadData(laCode, new(
                [], 
                "Download data"))
                .ToActionResult(View, _hostEnvironment);
        }
        protected override BreadcrumbTrailViewModel GetLandingPageBreadcrumbs(string laCode, string laName) => new([
            new("All local authorities", $"/local-authorities/")
        ], laName);

        protected override IEnumerable<BreadcrumbItem> GetChildPageBreadcrumbs(string laCode, string laName) => [
            new("All local authorities", $"/local-authorities/"),
            new(laName, $"/local-authority/{laCode}/"),
        ];

        protected override Task<Result<LocalAuthorityPageViewModel>> GetLocalAuthorityPage(string laCode, string laName,
            BreadcrumbTrailViewModel breadcrumbs)
        {
            var viewModel = new LocalAuthorityPageViewModel(
                laName,
                laName,
                breadcrumbs
            );

            return Task.FromResult(Result.Success(viewModel));
        }
    }
}