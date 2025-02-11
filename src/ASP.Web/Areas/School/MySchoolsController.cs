using ASP.Api.Client;
using ASP.Core.Results;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.Search;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsController : SchoolsController
    {
        private readonly SchoolSearchController _searchController;

        public MySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IOptions<SearchOptions> searchOptions
        ) : base(api, hostEnvironment)
        {
            _searchController = new SchoolSearchController(
                nameof(Schools),
                "MySchools",
                [],
                _api,
                searchOptions.Value,
                makeSchoolUrl: urn => Url.Action(nameof(MySchoolsSchoolController.LandingPage), "MySchoolsSchool", new { urn }));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _searchController.BindContext(context);
        }

        [HttpGet($"{SchoolSearchController.SubRouteTemplate}")]
        public async Task<IActionResult> Schools(SearchParameters parameters)
        {
            var result =
                from scopeInfo in User.GetScopeInfoForRole()
                from action in _searchController.Handle(
                    scopeInfo,
                    parameters,
                    [],
                    model => View(new SchoolSearchPageViewModel(
                        new PageViewModel(
                            model.BreadcrumbTrail,
                            model.PageTitle,
                            model.PageSubtitle
                        ),
                        model.Search,
                        model.SearchResults
                    )),
                    new() {
                        [SearchSubActionType.AllListings] = new() { Title = "My schools" }
                    })
                select action;

            return await result
                .ToActionResult(_hostEnvironment);
        }
    }
}