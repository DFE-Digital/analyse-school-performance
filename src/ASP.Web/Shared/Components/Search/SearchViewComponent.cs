using ASP.Web.Features.Search;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Shared.Components.SearchResults
{
    public class SearchViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(SearchViewModel search, string childContent, string? searchFormWidth)
        {
            return Task.FromResult((IViewComponentResult) View(
                new SearchViewComponentModel(search, childContent, searchFormWidth)));
        }
    }
}