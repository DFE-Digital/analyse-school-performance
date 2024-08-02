using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Core.Results;
using ASP.Web.Core.Templating;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Home
{
    [Area("Home")]
    [Route("home")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class HomeController : Controller
    {
        const string CONTENT_TEMPLATE_ID = "home-page";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public HomeController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [Authorize]
        [HttpGet("/")]
        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string? revision)
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

            return await _api.ViewContentTemplate(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID, revision))
                .Map(template => ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, revision, template))
                .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                .ToActionResult(View, _hostEnvironment);
        }
    }
}
