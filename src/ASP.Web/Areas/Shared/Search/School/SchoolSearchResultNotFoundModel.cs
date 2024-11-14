namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchResultNotFoundModel
{
    public string SearchTerm { get; }
    public string SearchUrl { get; }
    public int TotalCount { get; }

    public SchoolSearchResultNotFoundModel(string searchTerm, string searchUrl, int totalCount)
    {
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
        TotalCount = totalCount;
    }
}