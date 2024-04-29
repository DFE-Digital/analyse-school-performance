using ASP.Application.UseCases.GetEstablishmentDetails;
using ASP.Core.Establishments;
using ASP.Web.Features.TermsOfUse;
using ASP.Core.Results;
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

        public SchoolController(ILogger<SchoolController> logger, IGetEstablishmentDetailsUseCase useCase)
        {
            _logger = logger;
            _useCase = useCase;
        }

        [HttpGet("{urn}")]
        public async Task<IActionResult> Index(string urn)
        {
            GetEstablishmentDetailsUseCaseRequest request = new(urn);

            return await _useCase.HandleRequest(request)
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
                })
                .ToActionResult(View);
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
