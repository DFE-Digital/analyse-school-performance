using ASP.Application;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
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
    [Route("school/{urn}")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericSchoolController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public async Task<IActionResult> LandingPage(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, LANDING_PAGE_CONTENT_TEMPLATE_ID, revision);
        }

        [HttpGet("other-reports")]
        public async Task<IActionResult> OtherReports(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision, "Other reports");
        }


        [HttpGet("useful-links")]
        public async Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision, "Useful links");
        }

        private async Task<IActionResult> EstablishmentDetailsWithTemplate(string urn, string contentTemplateId, string? revision, string? page = null)
        {
            return await _api.GetEstablishmentDetails(new GetEstablishmentDetailsRequest(urn))
                .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
                .Then(async establishmentDetailsModel => await _api.ViewContentTemplate(new ViewContentTemplateRequest(contentTemplateId, revision))
                    .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template))
                    .DefaultIf(error => error is NotFoundError, new ContentTemplateViewModel())
                    .Map(contentTemplateModel => 
                    {
                        var schoolName = !string.IsNullOrWhiteSpace(establishmentDetailsModel.Name) 
                            ? establishmentDetailsModel.Name 
                            : "Missing school name";
                        
                        return new SchoolViewModel(
                            establishmentDetailsModel.Name,
                            Request.Path,
                            $"/school/{urn}/",
                            establishmentDetailsModel,
                            contentTemplateModel,
                            page == null
                                ? new BreadcrumbTrailViewModel(schoolName)
                                : new BreadcrumbTrailViewModel(page).AddBreadcrumb(schoolName, $"/school/{urn}/"));
                    }))
                .ToActionResult(View, _hostEnvironment);
        }
    }
}
