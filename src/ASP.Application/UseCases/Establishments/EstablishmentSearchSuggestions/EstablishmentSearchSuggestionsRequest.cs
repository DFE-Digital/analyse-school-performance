namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsRequest
{
    public EstablishmentSearchSuggestionsRequest(string searchTerm, int? maxSuggestions = null)
    {
        SearchTerm = searchTerm;
        MaxSuggestions = maxSuggestions;
    }

    public string SearchTerm { get; set; }
    public int? MaxSuggestions { get; set; }
}