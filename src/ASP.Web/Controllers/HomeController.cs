using ASP.Web.Filters;
using ASP.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.Web.Controllers
{
    [Route("home")]
    [ServiceFilter<CheckCookies>]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {    
            return View();
        }

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
