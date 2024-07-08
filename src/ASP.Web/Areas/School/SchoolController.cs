using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.Templating;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class SchoolController : Controller
    {
        const string LANDING_PAGE_CONTENT_TEMPLATE_ID = "school-landing-page";
        const string USEFUL_LINKS_CONTENT_TEMPLATE_ID = "school-useful-links";
        const string OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID = "school-other-reports-ofsted";

        private readonly IAspApi _api;
        private readonly IHostEnvironment _hostEnvironment;

        public SchoolController(
            IAspApi api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("{urn}")]
        public async Task<IActionResult> Index(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, LANDING_PAGE_CONTENT_TEMPLATE_ID, revision);
        }

        [HttpGet("{urn}/phonics")]
        public async Task<IActionResult> Phonics(string urn)
        {
            return await EstablishmentDetails(urn, "Phonics").ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/key-stage-2")]
        public async Task<IActionResult> KeyStage2(string urn)
        {
            return await EstablishmentDetails(urn, "Key stage 2").ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/key-stage-4")]
        public async Task<IActionResult> KeyStage4(string urn)
        {
            return await EstablishmentDetails(urn, "Key stage 4").ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/mtc")]
        public async Task<IActionResult> Mtc(string urn)
        {
            return await EstablishmentDetails(urn, "Multiplication table check").ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/other-reports")]
        public async Task<IActionResult> OtherReports(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, OTHER_REPORTS_OFSTED_CONTENT_TEMPLATE_ID, revision, "Other reports");
        }

        [HttpGet("{urn}/qla")]
        public async Task<IActionResult> Qla(string urn)
        {
            return await EstablishmentDetails(urn, "Question level analysis").ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/useful-links")]
        public async Task<IActionResult> UsefulLinks(string urn, string? revision)
        {
            return await EstablishmentDetailsWithTemplate(urn, USEFUL_LINKS_CONTENT_TEMPLATE_ID, revision, "Useful links");
        }

        private async Task<Result<SchoolViewModel>> EstablishmentDetails(string urn, string page)
        {
            var breadcrumbTrail = new BreadcrumbViewModel(page)
            
                .AddBreadcrumb("My school", $"/school/{urn}");

            return await _api.GetEstablishmentDetails(new GetEstablishmentDetailsUseCaseRequest(urn))
                 .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
                 .Map(establishmentDetailsModel => new SchoolViewModel()
                 {
                     EstablishmentDetails = establishmentDetailsModel,
                     Breadcrumbs = breadcrumbTrail
                 });

        }

        private async Task<IActionResult> EstablishmentDetailsWithTemplate(string urn, string contentTemplateId, string? revision, string? page = null)
        {
            var defaultIfNotFound = new ContentTemplateViewModel
            {
                Views = []
            };

            var breadcrumbTrail = new BreadcrumbViewModel(page)
                .AddBreadcrumb("My school", $"/school/{urn}");

            return await _api.GetEstablishmentDetails(new GetEstablishmentDetailsUseCaseRequest(urn))
               .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
               .Then(async establishmentDetailsModel => await _api.ViewContentTemplate(new ViewContentTemplateRequest(contentTemplateId, revision))
                   .Map(template => ContentTemplateViewModel.FromTemplate(contentTemplateId, revision, template))
                   .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                   .Map(contentTemplateModel => new SchoolViewModel
                   {
                       EstablishmentDetails = establishmentDetailsModel,
                       ContentTemplate = contentTemplateModel,
                       Breadcrumbs = breadcrumbTrail
                   }))
               .ToActionResult(View, _hostEnvironment);
        }
    }
}
