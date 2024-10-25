using ASP.Application;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Core.BreadcrumbTrail;
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

            var result =
                from results in GetAllEstablishments(ScopeType.All, Optional<string>.None, pageNumber)
                select DefaultViewModel(
                    results,
                    "All schools",
                    $"{results.TotalResults:N0} schools",
                    "/schools/",
                    GetSchoolsPageBreadcrumbs("All schools"));

            return result.ToActionResult(View, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }
    }
}