using ASP.Application;
using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Area("LocalAuthority")]
    [Route("local-authorities")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritiesController : Controller
    {
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericLocalAuthoritiesController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public Task<IActionResult> LocalAuthorities(string? page)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            
            return GetAllLocalAuthorities(pageNumber).Map(DefaultViewModel).ToActionResult(View, _hostEnvironment);
        }
        
        private LocalAuthoritiesPageViewModel DefaultViewModel(ResultsPage<LocalAuthorityDTO> result)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel("All local authorities");
        
            return new LocalAuthoritiesPageViewModel(
                "All local authorities",
                $"{result.TotalResults:N0} local authorities",
                result.TotalResults,
                new PaginationModel(
                    Url.Action(nameof(LocalAuthorities)) ?? "",
                    result.Page,
                    result.TotalResults,
                    result.ResultsPerPage,
                    "local authority",
                    "local authorities"
                ),
                result.Results,
                breadcrumbTrail
            );
        }
        private Task<Result<ResultsPage<LocalAuthorityDTO>>> GetAllLocalAuthorities(int pageNumber)
        {
            var request = new GetAllLocalAuthoritiesRequest(
                Optional<int>.Some(pageNumber),
                Optional<int>.Some(Constants.SearchResultPageSize)
            );

            return _api.GetAllLocalAuthorities(request).MapError(e => e is NotFoundError ? Error.Unexpected(e.Message, null): e);
        }
    }
}
