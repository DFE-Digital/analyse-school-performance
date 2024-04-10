using System.Diagnostics;
using ASP.Application.UseCases.ViewContentTemplate;
using ASP.Core.Results;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.Home
{
    [Area("Home")]
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
    }
}
