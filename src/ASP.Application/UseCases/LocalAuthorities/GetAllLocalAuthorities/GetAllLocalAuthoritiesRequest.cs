using ASP.Core.Optionality;

namespace ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;

public class GetAllLocalAuthoritiesRequest
{
    public Optional<int> Page { get; set; }
    public Optional<int> ResultsPerPage { get; set; }

    public GetAllLocalAuthoritiesRequest(Optional<int> page, Optional<int> resultsPerPage)
    {
        Page = page;
        ResultsPerPage = resultsPerPage;
    }
}