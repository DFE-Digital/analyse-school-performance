using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class SchoolController : Controller
    {
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(ILogger<SchoolController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
