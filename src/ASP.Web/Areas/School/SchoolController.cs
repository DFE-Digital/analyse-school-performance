using System.Text.RegularExpressions;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Establishments;
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
            var contentId = "school-landing-page";
            // Create the requests
            GetEstablishmentDetailsUseCaseRequest establishmentDetailsUseCaseRequest = new(urn);
            ViewContentTemplateRequest viewContentTemplateRequest = new(contentId);
            
            var viewContentTemplateResult = await _viewContentUseCase.HandleRequest(viewContentTemplateRequest)
                .Map(t => ContentTemplateViewModel.FromTemplate(contentId, t));

            var establishmentDetailsResult = await _useCase.HandleRequest(establishmentDetailsUseCaseRequest)
                .Map(t => new EstablishmentDetailsViewModel {
                    Urn = t.Urn,
                    Name = t.Name,
                    PhaseOfEducation = GetPhaseOfEducation(t),
                    Address = t.Address,
                    EstablishmentType = t.EstablishmentType,
                    Gender = t.Gender,
                    OfstedRating = t.OfstedRating,
                    OfstedLastInspectionDate = t.OfstedLastInspectionDate,
                    LocalAuthority = t.LocalAuthority,
                    HeadTeacher = t.HeadTeacher,
                    AgeRange = t.AgeRange,
                    ReligiousDenomination = t.ReligiousDenomination,
                    AdmissionsPolicy = t.AdmissionsPolicy,
                    ResourcedProvisionType = t.ResourcedProvisionType,
                    NoOfPupils = t.NoOfPupils,
                    IsDeleted = t.IsDeleted
                });
            if ((establishmentDetailsResult.IsSuccess) && (establishmentDetailsResult.Value?.IsDeleted ?? false))
            {
                return NotFound("Provided school urn is already removed");
            }
            if (establishmentDetailsResult.IsSuccess && viewContentTemplateResult.IsError)
            {
                var vm = new SchoolViewModel()
                {
                    EstablishmentDetails = establishmentDetailsResult.Value!,
                    ContentTemplate = new ContentTemplateViewModel()
                };
                return View("~/Areas/School/Index.cshtml", vm);
            }
            
            if (establishmentDetailsResult.IsError)
            {
                return await establishmentDetailsResult.ToTask().ToActionResult(View, _hostEnvironment);
            }
            
            var combinedResult = viewContentTemplateResult.Combine(establishmentDetailsResult,
                (viewContent,  establishmentDetails) => 
                    new SchoolViewModel() { ContentTemplate = viewContent, EstablishmentDetails = establishmentDetails });

            if (!combinedResult.IsSuccess) return await combinedResult.ToTask().ToActionResult(View, _hostEnvironment);
            
            return await combinedResult.ToTask().ToActionResult(View, _hostEnvironment);

        }
        
        private static string GetPhaseOfEducation(EstablishmentDetails establishmentDetails)
        {
            var phaseOfEducation = "";
            if (establishmentDetails.IsPrimary) phaseOfEducation = "Primary";
            if (establishmentDetails.IsSecondary) phaseOfEducation = "Secondary";
            if (establishmentDetails.IsPost16) phaseOfEducation = "16 to 18";
            return phaseOfEducation;
        }
    }
}
