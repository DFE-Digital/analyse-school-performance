using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchRequest
{
    public EstablishmentSearchRequest(string searchTerm, Scope scope, int? page, int? resultsPerPage)
    {
        SearchTerm = searchTerm;
        Scope = scope;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public string SearchTerm { get; set; }
    public Scope Scope { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
}