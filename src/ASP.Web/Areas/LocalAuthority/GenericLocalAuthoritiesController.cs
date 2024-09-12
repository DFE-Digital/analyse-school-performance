using ASP.Application;
using ASP.Web.Areas.LocalAuthority;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("LocalAuthority")]
    [Route("local-authorities")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritiesController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericLocalAuthoritiesController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public IActionResult LocalAuthorities()
        {
            return View(new LocalAuthoritiesPageViewModel(
                "All local authorities",
                new BreadcrumbTrailViewModel("All local authorities")
            ));
        }
    }
}
