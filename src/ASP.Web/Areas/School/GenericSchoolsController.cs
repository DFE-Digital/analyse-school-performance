using ASP.Application;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolsController : SchoolsController
    {
        public GenericSchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> Schools(string? page)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);

            return GetAllEstablishments(ScopeType.All, Optional<string>.None, pageNumber)
                .Map(results =>
                    DefaultViewModel(results, "All schools", $"{results.TotalResults:N0} schools", "/schools/"))
                .ToActionResult(View, _hostEnvironment);
        }
    }
}