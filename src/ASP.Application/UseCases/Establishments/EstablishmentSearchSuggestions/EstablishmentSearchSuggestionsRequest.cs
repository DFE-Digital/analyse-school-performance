using ASP.Core.Scope;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsRequest
{
    public EstablishmentSearchSuggestionsRequest(string searchTerm, ScopeType scopeType, string scopeIdentifier, int? maxSuggestions = null)
    {
        SearchTerm = searchTerm;
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        MaxSuggestions = maxSuggestions;
    }

    public ScopeType ScopeType { get; set; }
    public string ScopeIdentifier { get; set; }
    public string SearchTerm { get; set; }
    public int? MaxSuggestions { get; set; }
}