using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsController : SchoolsController
    {
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

        public MySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository
        ) : base(api, hostEnvironment)
        {
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        [HttpGet("")]
        public Task<IActionResult> Schools(string? page)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            var organisationName = User.FindFirst(CustomClaimTypes.OrganisationName)?.Value;
            var searchSuggestionsUrl = $"/search/suggestions/";

            var result =
                from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository,
                    _multiAcademyTrustRepository)
                from results in GetAllEstablishments(scopeInfo.ScopeType, scopeInfo.ScopeId, pageNumber)
                select DefaultViewModel(
                    results,
                    new SchoolsPageSearchParameters("My schools", $"{organisationName} - {results.TotalResults:N0} schools",
                        $"/my-schools/", GetSchoolsPageBreadcrumbs("My schools"),
                        "MySchools", nameof(Schools), searchSuggestionsUrl));

            return result.ToActionResult(View, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }
    }
}