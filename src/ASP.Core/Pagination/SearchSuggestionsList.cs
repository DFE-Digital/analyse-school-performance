namespace ASP.Core.Pagination;

public class SearchSuggestionsList<T>
{
    public IEnumerable<T> Suggestions { get; set; } = new List<T>();
    public int MaxSuggestions { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}