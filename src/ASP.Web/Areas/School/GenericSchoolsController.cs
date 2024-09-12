using ASP.Application;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolsController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericSchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public IActionResult Schools()
        {
            return View(new SchoolsPageViewModel(
                "All schools",
                new BreadcrumbTrailViewModel("All schools")
            ));
        }
    }
}
