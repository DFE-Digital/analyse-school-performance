namespace ASP.Core.Establishments.SearchSuggestions;

public class SearchSuggestionsResult<T>
{
    public IEnumerable<T> Suggestions { get; set; } = new List<T>();
    public int MaxSuggestions { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string ScopeIdentifier { get; set; } = string.Empty;
}