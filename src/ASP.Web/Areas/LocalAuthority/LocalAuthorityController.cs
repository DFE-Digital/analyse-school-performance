using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.LocalAuthority;
using ASP.Core.Results;
using ASP.Web.Core.Templating;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class LocalAuthorityController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public LocalAuthorityController(IAspApiClient api,
            IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("{laCode}")]
        public async Task<IActionResult> Index(string laCode)
        {
            return await LocalAuthorityWithTemplate(laCode, LANDING_PAGE_CONTENT_TEMPLATE_ID,
                "My local authority");
        }

        private async Task<IActionResult> LocalAuthorityWithTemplate(string laCode, string contentTemplateId, 
            string? page = null)
        {
            var defaultIfNotFound = new ContentTemplateViewModel
            {
                Views = []
            };

            var breadcrumbTrail = new BreadcrumbViewModel(page);

            return await _api.GetLocalAuthority(new GetLocalAuthorityRequest(laCode))
                .Then(async localAuthority => await _api
                    .ViewContentTemplate(new ViewContentTemplateRequest(contentTemplateId, null))
                    .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, null, template))
                    .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                    .Map(contentTemplateModel => new LocalAuthorityViewModel
                    {
                        Name = localAuthority.Name,
                        ContentTemplate = contentTemplateModel,
                        Breadcrumbs = breadcrumbTrail
                    }))
                .ToActionResult(View, _hostEnvironment);
        }
    }
}