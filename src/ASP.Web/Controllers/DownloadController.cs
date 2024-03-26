using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("download")]
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
