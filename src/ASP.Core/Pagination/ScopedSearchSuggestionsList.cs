namespace ASP.Core.Pagination;

public class ScopedSearchSuggestionsList<T> : SearchSuggestionsList<T>
{
    public string Scope { get; set; } = string.Empty;
    public string ScopeIdentifier { get; set; } = string.Empty;
}