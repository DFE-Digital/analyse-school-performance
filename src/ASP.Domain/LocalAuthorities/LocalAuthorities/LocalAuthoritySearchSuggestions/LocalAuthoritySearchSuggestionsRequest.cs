using ASP.Core.Optionality;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;

public class LocalAuthoritySearchSuggestionsRequest
{
    public string SearchTerm { get; }
    public Optional<int> MaxSuggestions { get; }

    public LocalAuthoritySearchSuggestionsRequest(string searchTerm, Optional<int> maxSuggestions)
    {
        SearchTerm = searchTerm;
        MaxSuggestions = maxSuggestions;
    }
}