using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsRequest
{
    public string SearchTerm { get; }
    public EstablishmentScopeType ScopeType { get; }
    public Optional<string> ScopeIdentifier { get; }
    public Optional<int> MaxSuggestions { get; }

    public EstablishmentSearchSuggestionsRequest(
        string searchTerm,
        EstablishmentScopeType scopeType,
        Optional<string> scopeIdentifier,
        Optional<int> maxSuggestions
    )
    {
        SearchTerm = searchTerm;
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        MaxSuggestions = maxSuggestions;
    }
}