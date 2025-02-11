using ASP.Api.Client;
using ASP.Api.Client.LocalAuthorities;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Web.Features.Search;
using ASP.Web.Features.SubController;
using ASP.Web.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    /// <summary>
    /// <para>
    ///     <c>LocalAuthoritySearchController</c> is a sub-controller that can be hosted within a controller action, 
    ///     allowing its routing and authorization to be determined by the hosting controller. 
    ///     The controller action specifies its route using attribute routing, but then delegates to the `LocalAuthoritySearchController` 
    ///     to resolve and handle the search sub-action based on the sub-route and query string parameters. 
    /// </para>
    /// </summary>
    /// 
    /// <remarks>
    /// <para>The action needs to handle GET methods:</para>
    /// <code>
    /// <![CDATA[
    /// // Adds search sub-action as a catch-all route parameter to the route template e.g. "local-authorities/{**subAction}"
    /// [HttpGet($"local-authorities/{LocalAuthoritySearchController.SubRouteTemplate}")]
    /// public Task<IActionResult> Search(SearchParameters parameters)
    /// {
    ///     ...
    /// }
    /// ]]>
    /// </code>
    /// <para>
    ///     <c>LocalAuthoritySearchController</c> has a <c>Handle()</c> method that returns a <c>Result&lt;IActionResult&gt;</c> - 
    ///     this can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions 
    ///     that may be needed to build up the page.
    /// </para>
    /// <para>
    ///     This needs to be passed a base breadcrumb trail that can be added to as the search actions are navigated, and a 
    ///     <c>showPageView</c> function to be called to render the page. This is so that the <c>LocalAuthoritySearchController</c> 
    ///     can take control of handling the search logic, including validation errors, not found errors (if the subAction 
    ///     part of the route is invalid), and search suggestions, rendering the page by calling the <c>showPageView</c> 
    ///     function as needed.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// public class LocalAuthoritiesController : Controller
    /// {
    ///     private readonly IAspApiClient api;
    ///     private readonly LocalAuthoritySearchController _localAuthoritySearchController;
    ///     
    ///     public class LocalAuthorityController(IAspApiClient api) 
    ///     {
    ///         _localAuthoritySearchController = new LocalAuthoritySearchController(
    ///             "Search",  // Action that will "host" the search sub-actions
    ///             "LocalAuthorities", // Host controller (i.e. this controller)
    ///             [],        // Keys of any route values the host action requires
    ///             _api);     // API client instance
    ///     }
    ///     
    ///     public override void OnActionExecuting(ActionExecutingContext context)
    ///     {
    ///         base.OnActionExecuting(context);
    ///
    ///         // Give the controller access to the currently executing ActionContext 
    ///         // in order to access route values and be able to redirect to sub-actions
    ///         _localAuthoritySearchController.BindContext(context);
    ///     }
    ///
    ///     [HttpGet($"local-authorities/{LocalAuthoritySearchController.SubRouteTemplate}")]
    ///     [HttpPost($"local-authorities/{LocalAuthoritySearchController.SubRouteTemplate}")]
    ///     public Task<IActionResult> Search(SearchParameters parameters)
    ///     {
    ///         var result =
    ///             from pageData in ... // get data needed to build the page
    ///         
    ///             // delegate to the LocalAuthoritySearchController to handle validation, error cases etc 
    ///             // and call the showPageView function to render the page
    ///             from actionResult in _localAuthoritySearchController.Handle(
    ///                 parameters,    // search parameters model-bound from route values/query string parameters
    ///                 Scope.Empty,   // scope parameter (not needed for LA search)
    ///                 
    ///                 // base breadcrumb trail for the Search page to be added to 
    ///                 // depending on the search sub-action
    ///                 this.GetSearchBaseBreadcrumbTrail() 
    ///                 
    ///                 // showPageView function to render a page view 
    ///                 // (displaying a download step, including any validation errors)
    ///                 model => View(
    ///                     // view model for this page that includes the download data step view model
    ///                     new SearchPageViewModel {
    ///                         BreadcrumbTrail = model.BreadcrumbTrail,
    ///                         PageTitle = model.StepTitle,
    ///                         Search = model.Search,
    /// 
    ///                         // other view model properties needed to build the page
    ///                     }))
    ///             select actionResult;
    ///    
    ///         return result
    ///             .ToActionResult(_hostEnvironment);
    ///     }
    /// }
    /// ]]>
    /// </code>
    ///
    /// <para>
    ///     On the page view side, the <asp-search /> tag helper just needs to be dropped into the view 
    ///     wherever the search page should be rendered. This should also be passed Razor markup to render
    ///     the listings for the search results. For example here's a dummy page view:
    /// </para>
    /// 
    /// <code>
    /// <![CDATA[
    /// @model SearchPageViewModel
    /// <partial name="_BreadcrumbTrail" model=@Model.BreadcrumbTrail />
    /// <h1>@Model.PageTitle</h1>
    /// /* ... other stuff */
    /// <asp-search model="Model.Search" search-form-width="one-half">
    ///    <partial name="_LocalAuthorityListingPartial" model="Model.LocalAuthorityListings" />
    /// </asp-search>
    /// ]]>
    /// </code>
    /// </remarks>
    [NonController]
    public class LocalAuthoritySearchController
        : SearchController<LocalAuthoritySearchController, LocalAuthoritySearchController.Scope, LocalAuthorityListingViewModel>
    {
        private readonly IAspApiClient _api;
        private readonly SearchOptions _searchOptions;

        /// <summary>
        /// Creates a <c>LocalAuthoritySearchController</c> instance to handle localAuthority searches.
        /// </summary>
        /// <param name="hostAction">Action that will "host" the search sub-actions</param>
        /// <param name="hostController">Controller the "host" action belongs to</param>
        /// <param name="hostActionRouteValueKeys">Keys of any route values the host action requires</param>
        /// <param name="api">API client instance</param>
        /// <param name="searchOptions">searchOptions</param>
        /// <param name="subActionRouteConfig">Optional config to override the default paths for each sub-action</param>
        public LocalAuthoritySearchController(
            string hostAction,
            string hostController,
            List<string> hostActionRouteValueKeys,
            IAspApiClient api,
            SearchOptions searchOptions,
            Dictionary<SearchSubActionType, SubActionRouteConfig>? subActionRouteConfig = null)
            : base(
                hostAction,
                hostController,
                hostActionRouteValueKeys,
                "local authority",
                "local authorities",
                Constants.LaSearchTermInputValidationMessage,
                subActionRouteConfig
            )
        {
            _api = api;
            _searchOptions = searchOptions;
        }

        protected override Task<Result<ResultsPage<LocalAuthorityListingViewModel>>> GetAllListings(
            Scope scope,
            int pageNumber)
        {
            var request = new GetAllLocalAuthoritiesRequest(
                pageNumber,
                _searchOptions.PageSize
            );

            return _api.GetAllLocalAuthorities(request)
                .Map(results => results.Map(MakeListingViewModel));
        }

        protected override Task<Result<SearchResultsPage<LocalAuthorityListingViewModel>>> PerformSearch(
            Scope scope,
            SearchParameters searchParams,
            int pageNumber)
        {
            var request = new LocalAuthoritySearchRequest(
                SearchTerm: searchParams.Search ?? string.Empty,
                Page: pageNumber,
                ResultsPerPage: _searchOptions.PageSize
            );

            return _api.LocalAuthoritySearch(request)
                .Map(results => results.Map(MakeListingViewModel));
        }

        protected override Task<Result<SearchSuggestionsList<LocalAuthorityListingViewModel>>> PerformSearchSuggestions(
            Scope scope,
            SearchParameters searchParams)
        {
            var request = new LocalAuthoritySearchSuggestionsRequest(
                searchParams.Search ?? "",
                _searchOptions.MaxSearchSuggestions
            );

            return _api.LocalAuthoritySearchSuggestions(request)
                .Map(results => results.Map(MakeListingViewModel));
        }

        protected override SearchFormViewModel CreateSearchFormViewModel(
            string searchTerm,
            string searchSuggestionUrl,
            PaginationViewModel pagination)
        {
            return SearchFormViewModel.ForLocalAuthorities(
                searchTerm,
                searchSuggestionUrl,
                pagination);
        }

        protected override string? GetListingUrl(LocalAuthorityListingViewModel listing)
            => MakeLocalAuthorityUrl(listing.Code);

        private string? MakeLocalAuthorityUrl(string laCode)
            => _urlHelper?.Action(nameof(GenericLocalAuthorityController.LandingPage), "GenericLocalAuthority", new { laCode });

        private LocalAuthorityListingViewModel MakeListingViewModel(
            LookupValueWithCode lookupValue)
        {
            return new LocalAuthorityListingViewModel {
                Code = lookupValue.Code,
                Name = lookupValue.Name,
                Url = MakeLocalAuthorityUrl(lookupValue.Code) ?? ""
            };
        }

        public class Scope
        {
            private Scope()
            {
            }

            public static Scope Empty => new Scope();
        }
    }
}