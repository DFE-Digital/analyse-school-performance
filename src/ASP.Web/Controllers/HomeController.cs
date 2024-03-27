using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Web.Extensions;
using ASP.Web.Models;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.Web.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly IViewContentTemplateUseCase _viewContentUseCase;

        public HomeController(IViewContentTemplateUseCase viewContentUseCase)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
        }

        [HttpGet("/")]
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            ViewContentTemplateRequest request = new("home-page");

            var defaultIfNotFound = new ContentTemplateViewModel
            {
                PageTitle = "Analyse school performance",
                PageContent = new {
                    HeroDescription = "Service description goes here..."
                },
                Views = []
            };

            var result = await _viewContentUseCase.HandleRequest(request)
                .Then(t => ContentTemplateViewModel.FromTemplate("home-page", t))
                .ToActionResult(View, defaultIfNotFound);

            return result;
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
