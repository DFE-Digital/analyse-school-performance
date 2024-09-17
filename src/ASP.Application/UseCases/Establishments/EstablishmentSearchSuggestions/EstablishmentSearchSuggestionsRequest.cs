using ASP.Core.Optionality;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsRequest
{
    public string SearchTerm { get; }
    public ScopeType ScopeType { get; }
    public Optional<string> ScopeIdentifier { get; }
    public Optional<int> MaxSuggestions { get; }

    public EstablishmentSearchSuggestionsRequest(
        string searchTerm, 
        ScopeType scopeType, 
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