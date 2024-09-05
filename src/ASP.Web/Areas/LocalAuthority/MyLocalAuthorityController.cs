using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Authorize(Policy = Policy.AccessToMyLocalAuthority)]
    [Area("LocalAuthority")]
    [Route("my-local-authority")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class MyLocalAuthorityController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "la-landing-page";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public MyLocalAuthorityController(IAspApiClient api, IHostEnvironment hostEnvironment)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return await LocalAuthorityWithTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID);
        }

        private async Task<IActionResult> LocalAuthorityWithTemplate(string contentTemplateId, string? page = null)
        {
            return await User.GetLocalAuthorityCode()
                .Then(laCode => _api.GetLocalAuthority(new(laCode))
                    .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                    .Then(async localAuthority => await _api.ViewContentTemplate(new(contentTemplateId, null))
                        .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, null, template))
                        .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel())
                        .Map(contentTemplateModel => new LocalAuthorityViewModel(
                            "My local authority",
                            localAuthority.Name,
                            contentTemplateModel,
                            page == null
                                ? new BreadcrumbTrailViewModel("My local authority")
                                : new BreadcrumbTrailViewModel(page).AddBreadcrumb("My local authority", $"/my-local-authority/")
                        ))))
                .ToActionResult(View, _hostEnvironment);
        }
    }
}