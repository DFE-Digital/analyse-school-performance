using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
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
        const string CONTENT_TEMPLATE_ID = "home-page";

        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IHostEnvironment _hostEnvironment;

        public HomeController(IViewContentTemplateUseCase viewContentUseCase, IHostEnvironment hostEnvironment)
        {
            _viewContentUseCase = viewContentUseCase ??
                throw new ArgumentNullException(nameof(viewContentUseCase));
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("/")]
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var defaultIfNotFound = new ContentTemplateViewModel
            {
                PageTitle = "Analyse school performance",
                PageContent = new
                {
                    HeroDescription = "Service description goes here..."
                },
                Views = []
            };

            return await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID))
                .Map(template => ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, template))
                .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                .ToActionResult(View, _hostEnvironment);
        }
    }
}
