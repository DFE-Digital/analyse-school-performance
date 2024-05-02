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

        public SchoolController(ILogger<SchoolController> logger, IGetEstablishmentDetailsUseCase useCase,
            IViewContentTemplateUseCase viewContentUseCase)
        {
            _logger = logger;
            _useCase = useCase;
            _viewContentUseCase = viewContentUseCase;
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

            viewContentTemplateResult = UpdateContentTemplateSchoolUri(viewContentTemplateResult, "136028", urn);

            var establishmentDetailsResult = await _useCase.HandleRequest(establishmentDetailsUseCaseRequest)
                .Map(t => new EstablishmentDetailsViewModel {
                    Urn = t.Urn,
                    Name = t.Name,
                    PhaseOfEducation = GetPhaseOfEducation(t),
                    Address = t.Address,
                    EstablishmentType = t.EstablishmentType,
                    Gender = t.Gender,
                    OfstedRating = t.OfstedRating,
                    LastInspectionDate = t.LastInspectionDate,
                    LocalAuthority = t.LocalAuthority,
                    HeadTeacher = t.HeadTeacher,
                    AgeRange = t.AgeRange,
                    ReligiousDenomination = t.ReligiousDenomination,
                    AdmissionsPolicy = t.AdmissionsPolicy,
                    ResourcedProvisionType = t.ResourcedProvisionType,
                    NoOfPupils = t.NoOfPupils
                });


            if (establishmentDetailsResult.IsSuccess && viewContentTemplateResult.IsError)
            {
                var vm = new SchoolViewModel()
                {
                    EstablishmentDetails = establishmentDetailsResult.Value!,
                    ContentTemplate = new ContentTemplateViewModel()
                };
                return View("~/Areas/School/Index.cshtml", vm);
            }
            
            if (viewContentTemplateResult.IsSuccess && establishmentDetailsResult.IsError)
            {
                var vm = new SchoolViewModel()
                {
                    EstablishmentDetails = new EstablishmentDetailsViewModel(),
                    ContentTemplate = viewContentTemplateResult.Value!
                };
                return View("~/Areas/School/Index.cshtml", vm);
            }
            
            var combinedResult = viewContentTemplateResult.Combine(establishmentDetailsResult,
                (viewContent,  establishmentDetails) => 
                    new SchoolViewModel() { ContentTemplate = viewContent, EstablishmentDetails = establishmentDetails });

            if (!combinedResult.IsSuccess) return await combinedResult.ToTask().ToActionResult(View);
            
            combinedResult = FilterSecondarySchoolResultView(combinedResult);

            return await combinedResult.ToTask().ToActionResult(View);

        }

        /// <summary>
        /// Filters the views within a SchoolViewModel based on the education phase. If the phase is 'Secondary', 
        /// it retains only specific views identified by their IDs. This method is intended for use with SchoolViewModel 
        /// instances that include detailed view components, ensuring that only relevant data is presented for secondary schools.
        /// </summary>
        /// <param name="combinedResult">A Result object containing a SchoolViewModel which may contain multiple views.</param>
        /// <returns>A Result object containing the filtered SchoolViewModel if the phase of education is 'Secondary'; 
        /// otherwise, returns the original unfiltered Result object.</returns>
        private static Result<SchoolViewModel> FilterSecondarySchoolResultView(Result<SchoolViewModel> combinedResult)
        {
            var phaseOfEducation = combinedResult.Value?.EstablishmentDetails.PhaseOfEducation;

            if (string.IsNullOrEmpty(phaseOfEducation) || !phaseOfEducation.Equals("Secondary"))
            {
                return combinedResult;
            }
    
            // List of IDs to filter for secondary views
            var filterIds = new HashSet<string>
            {
                "app-card-useful-links", 
                "app-card-other-reports", 
                "app-card-key-stage-4",
                "app-card-qla", 
            };
            
            // Filter the Views to only include specified IDs for secondary views
            combinedResult.Value!.ContentTemplate.Views = combinedResult.Value.ContentTemplate.Views
                .Where(view => filterIds.Contains(view.ViewContent.Id))
                .ToList();
            return combinedResult;
        }
        
        /// <summary>
        /// Updates the URIs within the content template views by replacing the old school Unique Reference Number (URN) with the provided new URN.
        /// </summary>
        /// <param name="contentTemplate">The content template result containing views to be updated.</param>
        /// <param name="oldId">The old school URN to replace.</param>
        /// <param name="newId">The new school URN to be used in replacement.</param>
        /// <returns>A result containing the updated content template with modified URIs.</returns>
        private static Result<ContentTemplateViewModel> UpdateContentTemplateSchoolUri(Result<ContentTemplateViewModel> contentTemplate, string oldId, string newId)
        {
            if (!contentTemplate.IsSuccess || contentTemplate.Value?.Views == null) return contentTemplate;

            contentTemplate.Value.Views.ForEach(view =>
            {
                dynamic viewContent = view.ViewContent;
                string linkUrl = viewContent.LinkUrl;
                if (!string.IsNullOrEmpty(linkUrl) && linkUrl.Contains(oldId))
                {
                    viewContent.LinkUrl = Regex.Replace(linkUrl, oldId, newId);
                }
            });

            return contentTemplate;
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
