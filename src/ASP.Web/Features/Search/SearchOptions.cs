using ASP.Core;

namespace ASP.Web.Features.Search;

public class SearchOptions
{
    public const string SectionName = "Search";

    public int PageSize { get; set; } = Constants.SearchResultPageSize;
    public int MaxSearchSuggestions { get; set; } = Constants.SearchResultMaxSuggestions;

}