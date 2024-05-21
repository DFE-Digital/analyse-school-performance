using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Web.Features.TermsOfUse;
using ASP.Core.Results;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Core.Templating;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class SchoolController : Controller
    {
        const string CONTENT_TEMPLATE_ID = "school-landing-page";

        private readonly ILogger<SchoolController> _logger;
        private readonly IGetEstablishmentDetailsUseCase _useCase;
        private readonly IViewContentTemplateUseCase _viewContentUseCase;
        private readonly IHostEnvironment _hostEnvironment;

        public SchoolController(ILogger<SchoolController> logger, IGetEstablishmentDetailsUseCase useCase,
            IViewContentTemplateUseCase viewContentUseCase, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _useCase = useCase;
            _viewContentUseCase = viewContentUseCase;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{urn}")]
        public async Task<IActionResult> Index(string urn)
        {
            var defaultIfNotFound = new ContentTemplateViewModel
            {
                Views = []
            };

            return await _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn))
                .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))
                .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
                .Then(async establishmentDetailsModel => await _viewContentUseCase.HandleRequest(new ViewContentTemplateRequest(CONTENT_TEMPLATE_ID))
                    .Map(template => ContentTemplateViewModel.FromTemplate(CONTENT_TEMPLATE_ID, template))
                    .DefaultIf(error => error is NotFoundError, defaultIfNotFound)
                    .Map(contentTemplateModel => new SchoolViewModel
                    {
                        EstablishmentDetails = establishmentDetailsModel,
                        ContentTemplate = contentTemplateModel
                    }))
                .ToActionResult(View, _hostEnvironment);

        }

        [HttpGet("{urn}/phonics")]
        public async Task<IActionResult> Phonics(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/key-stage-2")]
        public async Task<IActionResult> KeyStage2(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/key-stage-4")]
        public async Task<IActionResult> KeyStage4(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/mtc")]
        public async Task<IActionResult> Mtc(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/other-reports")]
        public async Task<IActionResult> OtherReports(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/qla")]
        public async Task<IActionResult> Qla(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        [HttpGet("{urn}/useful-links")]
        public async Task<IActionResult> UsefulLinks(string urn)
        {
            return await EstablishmentDetails(urn).ToActionResult(View, _hostEnvironment);
        }

        private async Task<Result<SchoolViewModel>> EstablishmentDetails(string urn)
        {
            return await _useCase.HandleRequest(new GetEstablishmentDetailsUseCaseRequest(urn))
                 .ErrorIf(estab => estab.IsDeleted, Error.NotFound($"Establishment {urn} has been deleted."))
                 .Map(EstablishmentDetailsViewModel.FromEstablishmentDetails)
                 .Map(establishmentDetailsModel => new SchoolViewModel() { EstablishmentDetails = establishmentDetailsModel });

        }
    }
}
