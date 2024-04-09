using ASP.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("download")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class DownloadController : Controller
    {
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(ILogger<DownloadController> logger)
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
