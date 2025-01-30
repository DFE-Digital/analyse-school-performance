using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch;

public class EstablishmentSearchRequest
{
    public string SearchTerm { get; }
    public EstablishmentScopeType ScopeType { get; }
    public Optional<string> ScopeIdentifier { get; }
    public Optional<int> Page { get; }
    public Optional<int> ResultsPerPage { get; }

    public EstablishmentSearchRequest(
        string searchTerm,
        EstablishmentScopeType scopeType,
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