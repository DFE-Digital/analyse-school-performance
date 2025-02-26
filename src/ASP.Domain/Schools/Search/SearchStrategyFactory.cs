using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search;

public class SearchStrategyFactory
{
    private readonly ISchoolRepository _repository;

    public SearchStrategyFactory(ISchoolRepository repository)
    {
        _repository = repository;
    }

    public SearchStrategy CreateStrategy(
        Optional<SchoolAccessScope> scope,
        SearchType searchType,
        string searchTerm,
        int page,
        int resultsPerPage)
    {
        switch (searchType)
        {
            case SearchType.Urn:
                return new UrnLookupStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            case SearchType.LAEstabCodeWithSeparator:
            case SearchType.LAEstabCodeWithoutSeparator:
            case SearchType.LACode:
            case SearchType.EstabCode:
                return new LaEstabSearchStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            case SearchType.NameOrLocation:
                return new NameOrLocationSearchStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            default:
                throw new NotSupportedException("Search type not supported.");
        }
    }
}