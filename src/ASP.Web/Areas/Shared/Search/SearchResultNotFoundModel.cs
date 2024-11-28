namespace ASP.Web.Areas.Shared.Search;

public class SearchResultNotFoundModel
{
    public string SearchTerm { get; }
    public string SearchUrl { get; }
    public int TotalCount { get; }

    public SearchResultNotFoundModel(string searchTerm, string searchUrl, int totalCount)
    {
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
        TotalCount = totalCount;
    }
}