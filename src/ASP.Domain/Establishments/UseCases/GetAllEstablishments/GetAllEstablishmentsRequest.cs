using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public class GetAllEstablishmentsRequest
{
    public EstablishmentScopeType ScopeType { get; set; }
    public Optional<string> ScopeIdentifier { get; set; }
    public Optional<int> Page { get; set; }
    public Optional<int> ResultsPerPage { get; set; }

    public GetAllEstablishmentsRequest(EstablishmentScopeType scopeType, Optional<string> scopeIdentifier, Optional<int> page, Optional<int> resultsPerPage)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }
}