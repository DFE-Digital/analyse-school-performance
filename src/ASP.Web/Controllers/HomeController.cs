using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Core.Results;
using ASP.Web.Extensions;
using ASP.Web.Filters;
using ASP.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Controllers
{
    [Route("home")]
    [ServiceFilter<TermsOfUseActionFilter>]
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
            // uncomment to test ExceptionHandlerServerError
            throw new Exception();

            ViewContentTemplateRequest request = new("home-page");

            var defaultIfNotFound = new ContentTemplateViewModel
            {
                PageTitle = "Analyse school performance",
                PageContent = new
                {
                    HeroDescription = "Service description goes here..."
                },
                Views = []
            };

            var result = await _viewContentUseCase.HandleRequest(request)
                .Map(t => ContentTemplateViewModel.FromTemplate("home-page", t))
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
            return View("~/Views/Shared/Errors/ServerError.cshtml", new ErrorViewModel { ErrorCode = HttpContext.TraceIdentifier });
        }
    }
}
