namespace ASP.Web.Areas.Search;

public class SearchResultNotFoundModel
{
    public string SearchTerm { get; set; } = string.Empty;
    public int TotalCount { get; set; }
}