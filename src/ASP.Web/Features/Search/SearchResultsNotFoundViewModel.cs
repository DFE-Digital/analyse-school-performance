namespace ASP.Web.Features.Search;

public class SearchResultsNotFoundViewModel : SearchViewModel
{
    public string SearchTerm { get; }
    public string SearchUrl { get; }

    public SearchResultsNotFoundViewModel(string searchTerm, string searchUrl)
    {
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
    }
}