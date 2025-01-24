using ASP.Application;
using ASP.Core;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Features.Search;
using ASP.Core.Helpers;
using ASP.Core.Scoping;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Utilities;
using ASP.Web.Shared.Pagination;
using ASP.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;

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
    /// <para>The action needs to handle GET and POST methods:</para>
    /// <code>
    /// <![CDATA[
    /// // Adds search sub-action as a catch-all route parameter to the route template e.g. "schools/{**searchSubAction}"
    /// [HttpGet($"schools/{SchoolSearchController.SubRouteTemplate}")]
    /// [HttpPost($"schools/{SchoolSearchController.SubRouteTemplate}")]
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
    ///     can take control of handling the search logic, including validation errors, not found errors (if the searchSubAction 
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
    {
        public const string SubRouteTemplate = "{**searchSubAction}";

        private readonly string _hostAction;
        private readonly string _hostController;
        private readonly List<string> _hostActionRouteValueKeys;
        private readonly IAspApiClient _api;
        private readonly Func<string, string?> _makeSchoolUrl;
        private readonly Dictionary<SchoolSearchSubActionType, SchoolSearchSubAction> _subActions = new();
        private RouteValueDictionary? _hostActionRouteValues;
        private ActionContext? _context;
        private IUrlHelper? _urlHelper;
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
            Dictionary<SchoolSearchSubActionType, SchoolSearchSubActionRouteConfig>? subActionRouteConfig = null)
        {
            _hostAction = hostAction;
            _hostController = hostController;
            _hostActionRouteValueKeys = hostActionRouteValueKeys;
            _api = api;
            _makeSchoolUrl = makeSchoolUrl;

            _subActions = new()
            {
                [SchoolSearchSubActionType.AllSchools] =
                    new AllSchools(this, "", "All schools", ""),

                [SchoolSearchSubActionType.SearchSuggestions] =
                    new SearchSuggestions(this, "suggestions", "Search suggestions", ""),
            };

            // Sub action paths can be overridden if needed
            if (subActionRouteConfig != null)
            {
                foreach ((var key, var value) in subActionRouteConfig)
                {
                    if (value.Path != null)
                    {
                        _subActions[key].Path = value.Path.TrimEnd('/', ' ');
                    }
                }
            }

            _searchOptions = searchOptions;
        }

        /// <summary>
        /// This method should be called from OnActionExecuting() as the controller needs access to the currently executing
        /// ActionContext in order to access route values and be able to redirect to sub-actions.
        /// </summary>
        /// <param name="context"></param>
        public void BindContext(ActionContext context)
        {
            _context = context;
            var factory = context.HttpContext?.RequestServices?.GetRequiredService<IUrlHelperFactory>();
            _urlHelper = factory?.GetUrlHelper(context);
            _hostActionRouteValues = new RouteValueDictionary(
                _context.RouteData.Values.Where(kvp => _hostActionRouteValueKeys.Contains(kvp.Key)));
        }

        /// <summary>
        /// Gets the Url for the initial action of this controller, using the optional routeValues provided
        /// </summary>
        /// <param name="routeValues">Optional routeValues for the initial action</param>
        /// <returns></returns>
        public string GetInitialActionUrl(object? routeValues = null)
        {
            var subActionRouteValues = GetRouteValuesForSubAction(SchoolSearchSubActionType.AllSchools, new SearchParameters(), routeValues ?? new { });
            var action = _urlHelper?.Action(_hostAction, _hostController, subActionRouteValues);
            return action ?? "";
        }

        public Task<Result<IActionResult>> Handle(
            ScopeInfo scope,
            SearchParameters parameters,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<SchoolSearchSubActionViewModel, IActionResult> showPageView,
            Dictionary<SchoolSearchSubActionType, SchoolSearchSubActionConfig>? subActionConfig = null
        )
        {
            // Sub action titles/subtitles can be overridden if needed
            if (subActionConfig != null)
            {
                foreach ((var key, var value) in subActionConfig)
                {
                    if (value.Title != null)
                    {
                        _subActions[key].Title = value.Title;
                    }

                    if (value.Subtitle != null)
                    {
                        _subActions[key].Subtitle = $"{value.Subtitle} - ";
                    }
                }
            }

            // Route value sub-action in route template /path/to/download-data/{**searchSubAction} is resolved as null if searchSubAction portion is empty
            // so SearchSubAction parameter will be null for first sub-action, but we prefer to deal with it as an empty string
            var subActionPath = (parameters.SearchSubAction ?? "").TrimEnd('/', ' ');
            var subAction = _subActions.Values.FirstOrDefault(a => a.Path == subActionPath);

            if (subAction == null)
            {
                return Task.FromResult(Result.NotFound<IActionResult>($"School search sub-action \"{subActionPath}\" was not recognised."));
            }

            return subAction.Handle(scope, parameters, baseBreadcrumbTrail, showPageView);
        }

        private string? RequestMethod
            => _context?.HttpContext.Request.Method;

        public bool ModelStateIsValid
            => _context?.ModelState.IsValid ?? false;

        private void AddModelError(string key, string errorMessage)
            => _context?.ModelState.AddModelError(key, errorMessage);

        private Result<IActionResult> RedirectToSubAction(SchoolSearchSubActionType subActionType, SearchParameters parameters)
            => new RedirectToActionResult(_hostAction, _hostController, _subActions[subActionType].GetRouteValues(parameters));

        private BreadcrumbItem GetBreadcrumbForSubAction(SchoolSearchSubActionType subActionType, SearchParameters parameters)
        {
            var subAction = _subActions[subActionType];
            return new BreadcrumbItem(subAction.Title, GetUrlForSubAction(subActionType, parameters));
        }

        private string GetUrlForSubAction(SchoolSearchSubActionType subActionType, SearchParameters parameters)
            => _urlHelper?.Action(_hostAction, _hostController, _subActions[subActionType].GetRouteValues(parameters)) ?? "";

        private RouteValueDictionary GetRouteValuesForSubAction(SchoolSearchSubActionType subActionType, SearchParameters parameters)
            => (_hostActionRouteValues ?? [])
                .Merge(_subActions[subActionType].GetRouteValues(parameters));

        private RouteValueDictionary GetRouteValuesForSubAction(SchoolSearchSubActionType subActionType, SearchParameters parameters, object initialRouteValues)
            => (_hostActionRouteValues ?? [])
                .Merge(initialRouteValues)
                .Merge(_subActions[subActionType].GetRouteValues(parameters));

        private Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(
            ScopeInfo scope, int pageNumber)
        {
            var request = new GetAllEstablishmentsRequest(
                scope.ScopeType,
                scope.ScopeId,
                Optional<int>.Some(pageNumber),
                Optional<int>.Some(_searchOptions.PageSize)
            );

            return _api.GetAllEstablishments(request);
        }

        private Task<Result<ScopedSearchResultsPage<EstablishmentListingDTO>>> PerformEstablishmentSearch(
            ScopeInfo scopeInfo,
            SearchParameters searchParams,
            int pageNumber
        )
        {
            var request = new EstablishmentSearchRequest(
                searchTerm: searchParams.Search ?? string.Empty,
                scopeType: scopeInfo.ScopeType,
                scopeInfo.ScopeId,
                page: Optional<int>.Some(pageNumber),
                resultsPerPage: Optional<int>.Some(_searchOptions.PageSize)
            );

            return _api.EstablishmentSearch(request);
        }

        private Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> PerformEstablishmentSearchSuggestions(
            SearchParameters searchParams, ScopeInfo scopeInfo)
        {
            var request = new EstablishmentSearchSuggestionsRequest(
                searchParams.Search ?? "",
                scopeInfo.ScopeType,
                scopeInfo.ScopeId,
                Optional<int>.Some(_searchOptions.MaxSearchSuggestions)
            );

            return _api.EstablishmentSearchSuggestions(request);
        }

        private PaginationViewModel CreatePaginationModel(
            ResultsPage<EstablishmentListingDTO> result,
            string paginationUrl)
        {
            return new PaginationViewModel(
                paginationUrl,
                currentPage: result.Page,
                totalResults: result.TotalResults,
                resultsPerPage: result.ResultsPerPage,
                "school",
                "schools"
            );
        }

        private SchoolSearchSubActionViewModel GetResultsViewModel(string title, string subtitle, SearchParameters parameters, IEnumerable<BreadcrumbItem> baseBreadcrumbTrail, ResultsPage<EstablishmentListingDTO> establishments)
        {
            return new SchoolSearchSubActionViewModel(
                title,
                $"{subtitle}{establishments.TotalResults:N0} schools",
                new BreadcrumbTrailViewModel(
                    baseBreadcrumbTrail
                ),
                SearchFormViewModel.ForSchools(
                    parameters.Search ?? "",
                    GetUrlForSubAction(SchoolSearchSubActionType.SearchSuggestions, parameters),
                    CreatePaginationModel(establishments, GetUrlForSubAction(SchoolSearchSubActionType.AllSchools, parameters))
                ),
                EstablishmentListingViewModel.FromEstablishmentListingDto(establishments.Results, _makeSchoolUrl)
            );
        }

        private SchoolSearchSubActionViewModel GetResultsNotFoundViewModel(string title, string? searchTerm, IEnumerable<BreadcrumbItem> baseBreadcrumbTrail)
        {
            return new SchoolSearchSubActionViewModel(
                title,
                "",
                new BreadcrumbTrailViewModel(
                    baseBreadcrumbTrail
                ),
                new SearchResultsNotFoundViewModel(
                    searchTerm ?? "",
                    GetInitialActionUrl()
                ),
                EstablishmentListingViewModel.FromEstablishmentListingDto([], _makeSchoolUrl)
            );
        }

        private class AllSchools : SchoolSearchSubAction
        {
            public AllSchools(
                SchoolSearchController controller,
                string path,
                string title,
                string subtitle)
                : base(controller, path, title, subtitle)
            {
            }

            public override object GetRouteValues(SearchParameters parameters)
                => new { searchSubAction = "", search = parameters.Search };

            public override async Task<Result<IActionResult>> Handle(
                ScopeInfo scope,
                SearchParameters parameters,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<SchoolSearchSubActionViewModel, IActionResult> showPageView)
            {
                var pageNumber = PageHelper.ParsePageNumber(parameters.Page);

                if (string.IsNullOrEmpty(parameters.Search))
                {
                    if (Controller.RequestContainsQueryStringKey(nameof(parameters.Search)))
                    {
                        Controller.AddModelError(nameof(parameters.Search), Constants.SchoolSearchTermInputValidationMessage);
                    }

                    return
                        from model in await Controller.GetAllEstablishments(scope, pageNumber)
                            .Map(establishments => Controller.GetResultsViewModel(
                                Title,
                                Subtitle,
                                parameters,
                                baseBreadcrumbTrail,
                                establishments
                            ))
                            .DefaultIf(e => e is NotFoundError, Controller.GetResultsNotFoundViewModel(
                                "We found no schools",
                                parameters.Search,
                                baseBreadcrumbTrail
                                    // If no schools found, breadcrumb trail would be e.g. Home > We found no schools
                                    // so to make it nicer we add an additional "All schools" breadcrumb step
                                    // so breadcrumb trail is Home > All schools > We found no schools
                                    .Append(new(Title, Controller.GetInitialActionUrl()))
                            ))
                        select showPageView(model);
                }

                return
                    from model in await Controller.PerformEstablishmentSearch(scope, parameters, pageNumber)
                        .Map(establishments => Controller.GetResultsViewModel(
                            $"Search results for \"{parameters.Search}\"",
                            Subtitle,
                            parameters,
                            baseBreadcrumbTrail
                                // If no schools found, breadcrumb trail would be e.g. Home > Search results for {searchTerm}
                                // so to make it nicer we add an additional "All schools" breadcrumb step
                                // so breadcrumb trail is Home > All schools > Search results for {searchTerm}
                                .Append(new(Title, Controller.GetInitialActionUrl())),
                            establishments
                        ))
                        .DefaultIf(e => e is NotFoundError, Controller.GetResultsNotFoundViewModel(
                            $"We found no matches for \"{parameters.Search}\"",
                            parameters.Search,
                            baseBreadcrumbTrail
                                // If no schools found, breadcrumb trail would be e.g. Home > We found no matches for {searchTerm}
                                // so to make it nicer we add an additional "All schools" breadcrumb step
                                // so breadcrumb trail is Home > All schools > We found no matches for {searchTerm}
                                .Append(new(Title, Controller.GetInitialActionUrl()))
                        ))
                    select model.Search is SearchFormViewModel searchForm && searchForm.Pagination.TotalResults == 1
                        ? Controller.RedirectToSchool(model.Establishments.First().Urn)
                        : showPageView(model);
            }
        }

        private IActionResult RedirectToSchool(string urn)
            => new RedirectResult(_makeSchoolUrl(urn) ?? GetInitialActionUrl());

        private bool RequestContainsQueryStringKey(string key)
            => (_context?.HttpContext.Request.Query.Keys ?? [])
                .Any(k => k.Equals(key, StringComparison.InvariantCultureIgnoreCase));

        private class SearchSuggestions : SchoolSearchSubAction
        {
            public SearchSuggestions(
                SchoolSearchController controller,
                string path,
                string title,
                string subtitle)
                : base(controller, path, title, subtitle)
            {
            }

            public override object GetRouteValues(SearchParameters parameters)
                => new { searchSubAction = Path, search = parameters.Search };

            public override async Task<Result<IActionResult>> Handle(
                ScopeInfo scope,
                SearchParameters parameters,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<SchoolSearchSubActionViewModel, IActionResult> showPageView)
            {
                if (!Controller.ModelStateIsValid)
                {
                    return new JsonResult(new SearchSuggestionsResult<EstablishmentSuggestionDTO>());
                }

                return
                    from searchSuggestions in await Controller.PerformEstablishmentSearchSuggestions(parameters, scope)
                    select (IActionResult)new JsonResult(searchSuggestions);
            }
        }

        private abstract class SchoolSearchSubAction
        {
            public SchoolSearchController Controller { get; }
            public string Path { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }

            public SchoolSearchSubAction(
                SchoolSearchController controller,
                string path,
                string title,
                string subtitle)
            {
                Controller = controller;
                Path = path;
                Title = title;
                Subtitle = subtitle;
            }

            public abstract object GetRouteValues(SearchParameters parameters);

            public RouteValueDictionary GetRouteValues(SearchParameters parameters, object initialRouteValues)
            {
                return new RouteValueDictionary(initialRouteValues).Merge(GetRouteValues(parameters));
            }

            public abstract Task<Result<IActionResult>> Handle(
                ScopeInfo scope,
                SearchParameters parameters,
                IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
                Func<SchoolSearchSubActionViewModel, IActionResult> showPageView
            );
        }
    }
}