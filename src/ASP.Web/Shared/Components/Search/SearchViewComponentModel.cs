using ASP.Web.Features.Search;

namespace ASP.Web.Shared.Components.SearchResults
{
    public class SearchViewComponentModel
    {
        public SearchViewModel Search { get; }
        public string ChildContent { get; }
        public string? SearchFormWidth { get; }

        public SearchViewComponentModel(SearchViewModel search, string childContent, string? searchFormWidth)
        {
            Search = search;
            ChildContent = childContent;
            SearchFormWidth = searchFormWidth;
        }
    }
}
