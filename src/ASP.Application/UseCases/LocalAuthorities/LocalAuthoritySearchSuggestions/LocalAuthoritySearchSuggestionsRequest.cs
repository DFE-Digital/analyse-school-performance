using ASP.Core.Optionality;

namespace ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearchSuggestions;

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