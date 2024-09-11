using ASP.Core.Scope;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchRequest
{
    public EstablishmentSearchRequest(string searchTerm, ScopeType scopeType, string scopeIdentifier, int? page, int? resultsPerPage)
    {
        SearchTerm = searchTerm;
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public string SearchTerm { get; set; }
    public ScopeType ScopeType { get; set; }
    public string ScopeIdentifier { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
}