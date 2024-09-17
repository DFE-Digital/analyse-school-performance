using ASP.Core.Optionality;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.GetAllEstablishments;

public class GetAllEstablishmentsRequest
{
    public ScopeType ScopeType { get; set; }
    public Optional<string> ScopeIdentifier { get; set; }
    public Optional<int> Page { get; set; }
    public Optional<int> ResultsPerPage { get; set; }

    public GetAllEstablishmentsRequest(ScopeType scopeType, Optional<string> scopeIdentifier, Optional<int> page, Optional<int> resultsPerPage)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }
}