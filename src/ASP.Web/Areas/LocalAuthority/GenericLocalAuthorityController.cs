using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToAllLocalAuthorities)]
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class GenericLocalAuthorityController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericLocalAuthorityController(IAspApiClient api, IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string laCode)
        {
            return await LocalAuthorityWithTemplate(laCode, LANDING_PAGE_CONTENT_TEMPLATE_ID);
        }

        private async Task<IActionResult> LocalAuthorityWithTemplate(string laCode, string contentTemplateId, string? page = null)
        {
            return await _api.GetLocalAuthority(new GetLocalAuthorityRequest(laCode))
                .Then(async localAuthority => await _api
                    .ViewContentTemplate(new ViewContentTemplateRequest(contentTemplateId, null))
                    .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, null, template))
                    .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel())
                    .Map(contentTemplateModel =>
                    {
                        var localAuthorityName = !string.IsNullOrWhiteSpace(localAuthority.Name)
                            ? localAuthority.Name
                            : "Missing local authority name";

                        return new LocalAuthorityViewModel(
                            localAuthorityName,
                            localAuthorityName,
                            contentTemplateModel,
                            page == null
                                ? new BreadcrumbTrailViewModel(localAuthorityName)
                                : new BreadcrumbTrailViewModel(page).AddBreadcrumb(localAuthorityName, $"local-authority/{laCode}"));

                    }))
                .ToActionResult(View, _hostEnvironment);
        }
    }
}