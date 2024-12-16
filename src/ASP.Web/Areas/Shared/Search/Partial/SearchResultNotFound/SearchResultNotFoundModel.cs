namespace ASP.Web.Areas.Shared.Search.Partial.SearchResultNotFound;

public class SearchResultNotFoundModel
{
    public string Title { get; }
    public string SearchUrl { get; }
    public int TotalCount { get; }

    public SearchResultNotFoundModel(string title, string searchUrl, int totalCount)
    {
        Title = title;
        SearchUrl = searchUrl;
        TotalCount = totalCount;
    }
}