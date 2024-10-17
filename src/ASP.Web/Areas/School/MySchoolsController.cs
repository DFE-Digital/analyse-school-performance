using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.School.ViewModels;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;
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
    public class MySchoolsController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

        public MySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment, 
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        [HttpGet("")]
        public Task<IActionResult> Schools(string? page)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            
            return Scope.GetScopeInfoForRole(User, _localAuthorityRepository, _multiAcademyTrustRepository)
                .Then(scopeInfo => GetAllEstablishments(scopeInfo.ScopeType, scopeInfo.ScopeId, pageNumber)
                    .Map(DefaultViewModel))
                .ToActionResult(View, _hostEnvironment);
        }

        private SchoolsPageViewModel DefaultViewModel(ScopedResultsPage<EstablishmentListingDTO> result)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel("My schools");

            return new SchoolsPageViewModel(
                "My schools",
                result.TotalResults,
                new PaginationModel(
                    Url.Action(nameof(Index)) ?? "",
                    result.Page,
                    result.TotalResults,
                    result.ResultsPerPage,
                    "school or college",
                    "schools or colleges"
                ),
                breadcrumbTrail,
                EstablishmentListingModel.FromEstablishmentListingDto(result.Results)
            );
        }
        
        private Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(
             ScopeType scopeType, Optional<string> scopeId, int pageNumber)
        {
            var request = new GetAllEstablishmentsRequest(
                scopeType,
                scopeId,
                Optional<int>.Some(pageNumber),
                Optional<int>.Some(Constants.SearchResultPageSize)
            );

            return _api.GetAllEstablishments(request).DefaultIf(error => error is NotFoundError, 
                new ScopedResultsPage<EstablishmentListingDTO>());
        }
    }
}
