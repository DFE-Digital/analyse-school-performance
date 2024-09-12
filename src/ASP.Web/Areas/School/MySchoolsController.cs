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
    [Route("my-schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public MySchoolsController(
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
                "My schools",
                new BreadcrumbTrailViewModel("My schools")
            ));
        }
    }
}
