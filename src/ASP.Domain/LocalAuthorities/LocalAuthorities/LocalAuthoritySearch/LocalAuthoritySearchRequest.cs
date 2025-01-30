using ASP.Core.Optionality;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;

public class LocalAuthoritySearchRequest
{
    public string SearchTerm { get; }
    public Optional<int> Page { get; set; }
    public Optional<int> ResultsPerPage { get; set; }

    public LocalAuthoritySearchRequest(string searchTerm, Optional<int> page, Optional<int> resultsPerPage)
    {
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }
}