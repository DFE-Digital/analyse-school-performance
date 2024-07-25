namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchRequest
{
    public EstablishmentSearchRequest(string searchTerm, int? page, int? resultsPerPage)
    {
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public string SearchTerm { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
}