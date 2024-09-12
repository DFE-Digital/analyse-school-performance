using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Authorization;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Core.Templating;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchool)]
    public class MySchoolController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public MySchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public async Task<IActionResult> LandingPage(string? revision)
        {
            return await EstablishmentDetailsWithTemplate(LANDING_PAGE_CONTENT_TEMPLATE_ID, revision);
        }

        [HttpGet("other-reports")]
        public async Task<IActionResult> OtherReports(string? revision)
        {
            return await EstablishmentDetailsWithTemplate(OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision, "Other reports");
        }


        [HttpGet("useful-links")]
        public async Task<IActionResult> UsefulLinks(string? revision)
        {
            return await EstablishmentDetailsWithTemplate(USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision, "Useful links");
        }

        private async Task<IActionResult> EstablishmentDetailsWithTemplate(string contentTemplateId, string? revision, string? page = null)
        {
            return await User.GetEstablishmentUrn()
                .Then(urn => _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                   .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                   .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
                   .Then(async establishmentDetailsModel => await _api.ViewContentTemplate(new ViewContentTemplateRequest(contentTemplateId, revision))
                       .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template))
                       .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel())
                       .Map(contentTemplateModel => new SchoolViewModel(
                           "My school",
                           Request.Path,
                           "/my-school/",
                           establishmentDetailsModel,
                           contentTemplateModel,
                           page == null
                                ? new BreadcrumbTrailViewModel("My school")
                                : new BreadcrumbTrailViewModel(page).AddBreadcrumb("My school", $"/my-school/")
                       ))))
               .ToActionResult(View, _hostEnvironment);
        }
    }
}
