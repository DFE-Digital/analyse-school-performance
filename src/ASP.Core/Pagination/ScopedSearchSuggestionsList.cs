namespace ASP.Core.Pagination;

public class ScopedSearchSuggestionsList<T> : SearchSuggestionsList<T>
{
    public string Scope { get; set; } = string.Empty;
    public string ScopeIdentifier { get; set; } = string.Empty;

    public new ScopedSearchSuggestionsList<TNew> Map<TNew>(Func<T, TNew> mapFunction)
    {
        return new ScopedSearchSuggestionsList<TNew>() {
            Scope = Scope,
            ScopeIdentifier = ScopeIdentifier,
            MaxSuggestions = MaxSuggestions,
            SearchTerm = SearchTerm,
            Suggestions = Suggestions.Select(mapFunction).ToList()
        };
    }
}