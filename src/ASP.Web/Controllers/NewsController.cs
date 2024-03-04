using ASP.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("news")]
    [ServiceFilter<CheckCookies>]
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger)
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
