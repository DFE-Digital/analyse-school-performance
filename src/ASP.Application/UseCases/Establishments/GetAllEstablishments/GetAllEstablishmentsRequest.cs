using ASP.Core.Scope;

namespace ASP.Application.UseCases.Establishments.GetAllEstablishments;

public class GetAllEstablishmentsRequest
{
    public GetAllEstablishmentsRequest(ScopeType scopeType, string scopeIdentifier, int? page, int? resultsPerPage)
    {
        ScopeType = scopeType;
        ScopeIdentifier = scopeIdentifier;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public ScopeType ScopeType { get; set; }
    public string ScopeIdentifier { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
}