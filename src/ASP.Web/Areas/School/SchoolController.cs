using ASP.Application.UseCases.GetEstablishmentDetails;
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
                .Map(t => new EstablishmentDetailsViewModel{ Urn = t.Urn, Name = t.Name })
                .ToActionResult(View);
        }

    }
}
