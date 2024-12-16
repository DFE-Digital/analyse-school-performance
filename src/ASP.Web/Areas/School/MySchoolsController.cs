using ASP.Application;
using ASP.Core.Authorization;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
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
            
            var searchConfig = SearchConfiguration.ForSchools(  
                searchSuggestionUrl: searchSuggestionsUrl 
            ); 
            var result =
                from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository,
                    _multiAcademyTrustRepository)
                from establishments in GetAllEstablishments(scopeInfo.ScopeType, scopeInfo.ScopeId, pageNumber)
                select new SchoolSearchPageViewModel(
                    new SearchPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsPageBreadcrumbs("My schools"),
                            "My schools",
                            $"{organisationName} - {establishments.TotalResults:N0} schools"
                        ),
                        searchConfig.SearchForm,
                        CreatePaginationModel(establishments,$"/my-schools/")
                    ),
                    MapEstablishmentListings(establishments.Results,urn => Url.Action(nameof(MySchoolsSchoolController.LandingPage),
                        "MySchoolsSchool", new { Area = "School", urn }))
                );
            return result.ToActionResult(View, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }
    }
}