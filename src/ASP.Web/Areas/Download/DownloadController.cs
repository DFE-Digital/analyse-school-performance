using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Download
{
    [Area("Download")]
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
