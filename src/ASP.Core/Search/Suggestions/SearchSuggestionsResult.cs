namespace ASP.Core.Search.Suggestions;

public class SearchSuggestionsResult<T>
{
    public IEnumerable<T> Suggestions { get; set; } = new List<T>();
    public int MaxSuggestions { get; set; }
    public string SearchTerm { get; set; } = string.Empty;   
}