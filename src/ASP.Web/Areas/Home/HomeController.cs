using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Templating.UseCases.ViewContentTemplate;
using ASP.Web.Core.Templating;
using ASP.Web.Extensions;
using ASP.Web.Features.TermsOfUse;
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

        [HttpGet("/")]
        [HttpGet("")]
        [HttpGet("index")]
        public Task<IActionResult> Index(string? revision)
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

            var result =
                from template in _api.ViewContentTemplate(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID, Optional.FromNullable(revision)))
                select ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, revision, template);

            return result
                .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                .ToActionResult(View, _hostEnvironment);
        }
    }
}
