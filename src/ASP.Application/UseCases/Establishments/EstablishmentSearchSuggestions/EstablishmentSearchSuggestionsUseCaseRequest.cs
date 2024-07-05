namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsUseCaseRequest
{
    public EstablishmentSearchSuggestionsUseCaseRequest(string searchTerm, int maxSuggestions = Core.Constants.SearchResultMaxSuggestions)
    {
        SearchTerm = searchTerm;
        MaxSuggestions = maxSuggestions;
    }

    public string SearchTerm { get; set; }
    public int MaxSuggestions { get; set; }
}