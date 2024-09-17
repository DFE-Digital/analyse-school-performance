using ASP.Core.Optionality;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchRequest
{
    public string SearchTerm { get; }
    public ScopeType ScopeType { get; }
    public Optional<string> ScopeIdentifier { get; }
    public Optional<int> Page { get; }
    public Optional<int> ResultsPerPage { get; }

    public EstablishmentSearchRequest(
        string searchTerm, 
        ScopeType scopeType, 
        Optional<string> scopeIdentifier, 
        Optional<int> page, 
        Optional<int> resultsPerPage
    )
    {
        SearchTerm = searchTerm;
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }
}