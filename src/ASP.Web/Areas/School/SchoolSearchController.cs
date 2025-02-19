using ASP.Api.Client;
using ASP.Api.Client.Schools;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Web.Features.Search;
using ASP.Web.Features.SubController;
using ASP.Web.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    /// <summary>
    /// <para>
    ///     <c>SchoolSearchController</c> is a sub-controller that can be hosted within a controller action, 
    ///     allowing its routing and authorization to be determined by the hosting controller. 
    ///     The controller action specifies its route using attribute routing, but then delegates to the `SchoolSearchController` 
    ///     to resolve and handle the search sub-action based on the sub-route and query string parameters. 
    /// </para>
    /// </summary>
    /// 
    /// <remarks>
    /// <para>The action needs to handle GET methods:</para>
    /// <code>
    /// <![CDATA[
    /// // Adds search sub-action as a catch-all route parameter to the route template e.g. "schools/{**subAction}"
    /// [HttpGet($"schools/{SchoolSearchController.SubRouteTemplate}")]
    /// public Task<IActionResult> Search(SearchParameters parameters)
    /// {
    ///     ...
    /// }
    /// ]]>
    /// </code>
    /// <para>
    ///     <c>SchoolSearchController</c> has a <c>Handle()</c> method that returns a <c>Result&lt;IActionResult&gt;</c> - 
    ///     this can then be incorporated in a <c>Result&lt;&gt;</c> chain along with other <c>Result&lt;&gt;</c> actions 
    ///     that may be needed to build up the page.
    /// </para>
    /// <para>
    ///     This needs to be passed a base breadcrumb trail that can be added to as the search actions are navigated, and a 
    ///     <c>showPageView</c> function to be called to render the page. This is so that the <c>SchoolSearchController</c> 
    ///     can take control of handling the search logic, including validation errors, not found errors (if the subAction 
    ///     part of the route is invalid), and search suggestions, rendering the page by calling the <c>showPageView</c> 
    ///     function as needed.
    /// </para>
    /// <code>
    /// <![CDATA[
    /// public class SchoolsController : Controller
    /// {
    ///     private readonly IAspApiClient api;
    ///     private readonly SchoolSearchController _schoolSearchController;
    ///     
    ///     public class SchoolController(IAspApiClient api) 
    ///     {
    ///         _schoolSearchController = new SchoolSearchController(
    ///             "Search",  // Action that will "host" the search sub-actions
    ///             "Schools", // Host controller (i.e. this controller)
    ///             [],        // Keys of any route values the host action requires
    ///             _api,      // API client instance
    ///             // function to create a school page URL from a URN, 
    ///             // to link to from the search results
    ///             urn => Url.Action("LandingPage", "GenericSchool", new { urn }));
    ///     }
    ///     
    ///     public override void OnActionExecuting(ActionExecutingContext context)
    ///     {
    ///         base.OnActionExecuting(context);
    ///
    ///         // Give the controller access to the currently executing ActionContext 
    ///         // in order to access route values and be able to redirect to sub-actions
    ///         _schoolSearchController.BindContext(context);
    ///     }
    ///
    ///     [HttpGet($"schools/{SchoolSearchController.SubRouteTemplate}")]
    ///     [HttpPost($"schools/{SchoolSearchController.SubRouteTemplate}")]
    ///     public Task<IActionResult> Search(SearchParameters parameters)
    ///     {
    ///         var result =
    ///             from scope in ...     // get scope info from somewhere
    ///             from otherData in ... // get other data needed to build the page
    ///         
    ///             // delegate to the DownloadDataController to handle validation, error cases etc 
    ///             // and call the showPageView function to render the page
    ///             from actionResult in _schoolSearchController.Handle(
    ///                 parameters,    // search parameters model-bound from route values/query string parameters
    ///                 scope,         // scope under which to do search
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
    ///    <partial name="_EstablishmentListingPartial" model="Model.EstablishmentListings" />
    /// </asp-search>
    /// ]]>
    /// </code>
    /// </remarks>
    [NonController]
    public class SchoolSearchController
        : SearchController<SchoolSearchController, SchoolsScopeInfo?, EstablishmentListingViewModel>
    {
        private readonly IAspApiClient _api;
        private readonly Func<string, string?> _makeSchoolUrl;
        private readonly SearchOptions _searchOptions;

        /// <summary>
        /// Creates a <c>SchoolSearchController</c> instance to handle school searches.
        /// </summary>
        /// <param name="hostAction">Action that will "host" the search sub-actions</param>
        /// <param name="hostController">Controller the "host" action belongs to</param>
        /// <param name="hostActionRouteValueKeys">Keys of any route values the host action requires</param>
        /// <param name="api">API client instance</param>
        /// <param name="searchOptions">searchOptions</param>
        /// <param name="makeSchoolUrl">Function to create a school page URL from a URN, to link to from the search results</param>
        /// <param name="subActionRouteConfig">Optional config to override the default paths for each sub-action</param>
        public SchoolSearchController(
            string hostAction,
            string hostController,
            List<string> hostActionRouteValueKeys,
            IAspApiClient api,
            SearchOptions searchOptions,
            Func<string, string?> makeSchoolUrl,
            Dictionary<SearchSubActionType, SubActionRouteConfig>? subActionRouteConfig = null)
            : base(
                hostAction,
                hostController,
                hostActionRouteValueKeys,
                "school",
                "schools",
                Constants.SchoolSearchTermInputValidationMessage,
                subActionRouteConfig
            )
        {
            _api = api;
            _makeSchoolUrl = makeSchoolUrl;
            _searchOptions = searchOptions;
        }

        protected override Task<Result<ResultsPage<EstablishmentListingViewModel>>> GetAllListings(
            SchoolsScopeInfo? scopeInfo,
            int pageNumber)
        {
            var request = new SchoolsGetAllRequest(
                SearchTerm: null,
                Scope: scopeInfo,
                Page: pageNumber,
                ResultsPerPage: _searchOptions.PageSize
            );

            return _api.SchoolsGetAll(request)
                .Map(results => results.Map(MakeListingViewModel));
        }

        protected override Task<Result<ResultsPage<EstablishmentListingViewModel>>> PerformSearch(
            SchoolsScopeInfo? scopeInfo,
            SearchParameters searchParams,
            int pageNumber)
        {
            var request = new SchoolsGetAllRequest(
                SearchTerm: searchParams.Search,
                Scope: scopeInfo,
                Page: pageNumber,
                ResultsPerPage: _searchOptions.PageSize
            );

            return _api.SchoolsGetAll(request)
                .Map(results => results.Map(MakeListingViewModel));
        }

        protected override Task<Result<List<EstablishmentListingViewModel>>> PerformSearchSuggestions(
            SchoolsScopeInfo? scopeInfo,
            SearchParameters searchParams)
        {
            var request = new SchoolsGetSearchSuggestionsRequest(
                SearchTerm: searchParams.Search ?? "",
                scopeInfo,
                _searchOptions.MaxSearchSuggestions
            );

            return _api.SchoolsGetSearchSuggestions(request)
                .Map(results => results
                    .Select(MakeListingViewModel).ToList());
        }

        protected override SearchFormViewModel CreateSearchFormViewModel(
            string searchTerm,
            string searchSuggestionUrl,
            PaginationViewModel pagination)
        {
            return SearchFormViewModel.ForSchools(
                searchTerm,
                searchSuggestionUrl,
                pagination);
        }

        protected override string? GetListingUrl(EstablishmentListingViewModel listing)
            => _makeSchoolUrl(listing.Urn);

        private EstablishmentListingViewModel MakeListingViewModel(
            SchoolListing listing
        )
        {
            return new EstablishmentListingViewModel {
                Name = listing.Name,
                Address = !string.IsNullOrEmpty(listing.Address) ? listing.Address : "No address available",
                Urn = listing.Urn,
                Laestab = !string.IsNullOrEmpty(listing.Laestab) ? listing.Laestab : "No data available",
                Url = _makeSchoolUrl(listing.Urn) ?? ""
            };
        }

        private EstablishmentListingViewModel MakeListingViewModel(
            SchoolSuggestion suggestion
        )
        {
            return new EstablishmentListingViewModel {
                Name = suggestion.Name,
                Address = !string.IsNullOrEmpty(suggestion.Address) ? suggestion.Address : "No address available",
                Urn = suggestion.Urn,
                Laestab = !string.IsNullOrEmpty(suggestion.Laestab) ? suggestion.Laestab : "No data available",
                Url = _makeSchoolUrl(suggestion.Urn) ?? ""
            };
        }
    }
}