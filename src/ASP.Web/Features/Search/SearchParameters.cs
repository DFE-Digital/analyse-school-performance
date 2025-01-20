namespace ASP.Web.Features.Search;

public class SearchParameters
{
    public static readonly List<string> RouteValueKeys = ["searchSubAction", "search", "page"];

    public string? SearchSubAction { get; set; }
    public string? Search { get; set; }
    public string? Page { get; set; }

    public RouteValueDictionary AsRouteValues()
        => new(new { searchSubAction = SearchSubAction, search = Search, page = Page });
}