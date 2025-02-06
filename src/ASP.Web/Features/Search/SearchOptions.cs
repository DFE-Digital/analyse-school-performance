namespace ASP.Web.Features.Search;

public class SearchOptions
{
    public const string SectionName = "Search";

    public int PageSize { get; set; } = ASP.Core.Constants.SearchResultPageSize;
    public int MaxSearchSuggestions { get; set; } = ASP.Core.Constants.SearchResultMaxSuggestions;

}