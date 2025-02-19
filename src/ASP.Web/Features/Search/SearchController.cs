using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.SubController;
using ASP.Web.Shared.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.Search;

/// <summary>
/// <para>
///     <c>SearchController</c> is a sub-controller (<see cref="SubController{TController, TParameters, TScope, TSubActionType, TSubActionViewModel}"/> that can be hosted within a controller action,
///     allowing its routing and authorization to be determined by the hosting controller. 
///     The controller action specifies its route using attribute routing, but then delegates to the `SearchController` 
///     to resolve and handle the search sub-action based on the sub-route and query string parameters. 
/// </para>
/// <para>
///     <c>SearchController</c> represents the concept of displaying listings, searching within them and providing search suggestions.
///     Derived classes provide the actual implementation details for getting the initial list, searching
///     and getting search suggestions. This class encapsulates the common functionality around searching, including how to handle
///     no results, appropriate breadcrumb trails, and rendering page titles and subtitles, so these are handled in a
///     consistent way across all items being searched.
/// </para>
/// </summary>
/// <typeparam name="TSearchController">Type of the derived controller inheriting from this class.</typeparam>
/// <typeparam name="TScope">Custom type representing the scope of the search (e.g. EstablishmentScope)</typeparam>
/// <typeparam name="TListingViewModel">Type of the view model for an individual listing.</typeparam>
[NonController]
public abstract class SearchController<TSearchController, TScope, TListingViewModel>
    : SubController<TSearchController, SearchParameters, TScope, SearchSubActionType, SearchSubActionViewModel<TListingViewModel>>
    where TSearchController : SearchController<TSearchController, TScope, TListingViewModel>
{
    private readonly string _listingNameSingular;
    private readonly string _listingNamePlural;
    private readonly string _searchTermInputValidationMessage;

    protected SearchController(
        string hostAction,
        string hostController,
        List<string> hostActionRouteValueKeys,
        string listingNameSingular,
        string listingNamePlural,
        string searchTermInputValidationMessage,
        Dictionary<SearchSubActionType, SubActionRouteConfig>? subActionRouteConfig = null) 
        : base(
            hostAction,
            hostController,
            hostActionRouteValueKeys,
            subActionRouteConfig)
    {
        _listingNameSingular = listingNameSingular;
        _listingNamePlural = listingNamePlural;
        _searchTermInputValidationMessage = searchTermInputValidationMessage;

        AddSubAction(SearchSubActionType.AllListings,
            new AllListings((TSearchController)this, "", $"All {_listingNamePlural}"));

        AddSubAction(SearchSubActionType.SearchSuggestions,
            new SearchSuggestions((TSearchController)this, "suggestions", "Search suggestions"));
    }

    protected abstract Task<Result<ResultsPage<TListingViewModel>>> GetAllListings(
        TScope scope,
        int pageNumber);

    protected abstract Task<Result<ResultsPage<TListingViewModel>>> PerformSearch(
        TScope scope,
        SearchParameters searchParams,
        int pageNumber);

    protected abstract Task<Result<List<TListingViewModel>>> PerformSearchSuggestions(
        TScope scope,
        SearchParameters searchParams);

    protected abstract SearchFormViewModel CreateSearchFormViewModel(
        string searchTerm,
        string searchSuggestionUrl,
        PaginationViewModel pagination);

    protected abstract string? GetListingUrl(TListingViewModel listing);

    protected class AllListings : SubAction
    {
        public AllListings(
            TSearchController controller,
            string path,
            string title,
            string subtitle = "")
            : base(controller, path, title, subtitle)
        {
        }

        public override RouteValueDictionary GetRouteValues(SearchParameters parameters)
            => base.GetRouteValues(parameters).Merge(new { search = parameters.Search });

        public override async Task<Result<IActionResult>> Handle(
            SearchParameters parameters,
            TScope scope,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<SearchSubActionViewModel<TListingViewModel>, IActionResult> showPageView)
        {
            var pageNumber = PageHelper.ParsePageNumber(parameters.Page);

            if (string.IsNullOrEmpty(parameters.Search))
            {
                if (Controller.RequestContainsQueryStringKey(nameof(parameters.Search)))
                {
                    Controller.AddModelError(nameof(parameters.Search), Controller._searchTermInputValidationMessage);
                }

                return
                    from model in await Controller.GetAllListings(scope, pageNumber)
                        .Map(listings => Controller.GetResultsViewModel(
                            Title,
                            Subtitle,
                            parameters,
                            baseBreadcrumbTrail,
                            listings
                        ))
                        .DefaultIf(e => e is NotFoundError, Controller.GetResultsNotFoundViewModel(
                            $"We found no {Controller._listingNamePlural}",
                            parameters.Search,
                            baseBreadcrumbTrail
                                // If no items found, breadcrumb trail would be e.g. Home > We found no {items}
                                // so to make it nicer we add an additional "All {items}" breadcrumb step
                                // so breadcrumb trail is Home > All {items} > We found no {items}
                                .Append(new(Title, Controller.GetInitialActionUrl()))
                        ))
                    select showPageView(model);
            }

            return
                from model in await Controller.PerformSearch(scope, parameters, pageNumber)
                    .Map(listings => Controller.GetResultsViewModel(
                        $"Search results for \"{parameters.Search}\"",
                        Subtitle,
                        parameters,
                        baseBreadcrumbTrail
                            // If no items found, breadcrumb trail would be e.g. Home > Search results for {searchTerm}
                            // so to make it nicer we add an additional "All {items}" breadcrumb step
                            // so breadcrumb trail is Home > All {items} > Search results for {searchTerm}
                            .Append(new(Title, Controller.GetInitialActionUrl())),
                        listings
                    ))
                    .DefaultIf(e => e is NotFoundError, Controller.GetResultsNotFoundViewModel(
                        $"We found no matches for \"{parameters.Search}\"",
                        parameters.Search,
                        baseBreadcrumbTrail
                            // If no items found, breadcrumb trail would be e.g. Home > We found no matches for {searchTerm}
                            // so to make it nicer we add an additional "All {items}" breadcrumb step
                            // so breadcrumb trail is Home > All {items}} > We found no matches for {searchTerm}
                            .Append(new(Title, Controller.GetInitialActionUrl()))
                    ))
                select model.Search is SearchFormViewModel searchForm && searchForm.Pagination.TotalResults == 1
                    ? new RedirectResult(Controller.GetListingUrl(model.SearchResults.First()) ?? Controller.GetInitialActionUrl())
                    : showPageView(model);
        }
    }

    protected class SearchSuggestions : SubAction
    {
        public SearchSuggestions(
            TSearchController controller,
            string path,
            string title,
            string subtitle = "")
            : base(controller, path, title, subtitle)
        {
        }

        public override RouteValueDictionary GetRouteValues(SearchParameters parameters)
            => base.GetRouteValues(parameters).Merge(new { search = parameters.Search });

        public override async Task<Result<IActionResult>> Handle(
            SearchParameters parameters,
            TScope scope,
            IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
            Func<SearchSubActionViewModel<TListingViewModel>, IActionResult> showPageView)
        {
            if (!Controller.ModelStateIsValid)
            {
                return new JsonResult(new List<TListingViewModel>());
            }

            return
                from searchSuggestions in await Controller.PerformSearchSuggestions(scope, parameters)
                select (IActionResult)new JsonResult(searchSuggestions);
        }
    }

    private SearchSubActionViewModel<TListingViewModel> GetResultsViewModel(
        string title,
        string subtitle,
        SearchParameters parameters,
        IEnumerable<BreadcrumbItem> baseBreadcrumbTrail,
        ResultsPage<TListingViewModel> listings)
    {
        return new SearchSubActionViewModel<TListingViewModel>(
            title,
            $"{subtitle}{listings.TotalResults:N0} {_listingNamePlural}",
            new BreadcrumbTrailViewModel(
                baseBreadcrumbTrail
            ),
            CreateSearchFormViewModel(
                parameters.Search ?? "",
                GetUrlForSubAction(SearchSubActionType.SearchSuggestions, parameters),
                CreatePaginationModel(listings, GetUrlForSubAction(SearchSubActionType.AllListings, parameters))
            ),
            listings.Results.ToList()
        );
    }

    private SearchSubActionViewModel<TListingViewModel> GetResultsNotFoundViewModel(
        string title,
        string? searchTerm,
        IEnumerable<BreadcrumbItem> baseBreadcrumbTrail)
    {
        return new SearchSubActionViewModel<TListingViewModel>(
            title,
            "",
            new BreadcrumbTrailViewModel(
                baseBreadcrumbTrail
            ),
            new SearchResultsNotFoundViewModel(
                searchTerm ?? "",
                GetInitialActionUrl()
            ),
            []
        );
    }

    private PaginationViewModel CreatePaginationModel(
        ResultsPage<TListingViewModel> result,
        string paginationUrl)
    {
        return new PaginationViewModel(
            paginationUrl,
            currentPage: result.Page,
            totalResults: result.TotalResults,
            resultsPerPage: result.ResultsPerPage,
            _listingNameSingular,
            _listingNamePlural
        );
    }
}