using ASP.Api.Client;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.Search;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ASP.Web.Areas.LocalAuthority
{
    [Area("LocalAuthority")]
    [Route("local-authorities")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritiesController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly LocalAuthoritySearchController _searchController;

        public GenericLocalAuthoritiesController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment, 
            IOptions<SearchOptions> searchOptions)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));

            _searchController = new LocalAuthoritySearchController(
                nameof(LocalAuthorities),
                "GenericLocalAuthorities",
                [],
                api,
                searchOptions.Value);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _searchController.BindContext(context);
        }

        [HttpGet($"{LocalAuthoritySearchController.SubRouteTemplate}")]
        public async Task<IActionResult> LocalAuthorities(SearchParameters parameters)
        {
            var result = await _searchController.Handle(
                LocalAuthoritySearchController.Scope.Empty,
                parameters,
                [],
                model => View(new LocalAuthoritySearchPageViewModel(
                    new PageViewModel(
                        model.BreadcrumbTrail,
                        model.PageTitle,
                        model.PageSubtitle
                    ),
                    model.Search,
                    model.SearchResults
                ))
            );

            return result
                .ToActionResult(_hostEnvironment);
        }
    }
}
