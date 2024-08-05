using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch;

public class EstablishmentSearchStrategyFactory : IEstablishmentSearchStrategyFactory
{
    private readonly IEstablishmentRepository _repository;
    private readonly ISearchService  _searchService;

    public EstablishmentSearchStrategyFactory(
        IEstablishmentRepository repository,
        ISearchService searchService)
    {
        _repository = repository;
        _searchService = searchService;
    }

    public EstablishmentSearchStrategy CreateStrategy(SearchType searchType, string searchTerm, int page, int resultsPerPage)
    {
        switch (searchType)
        {
            case SearchType.Urn:
            return new UrnLookupStrategy(_repository, searchTerm, page, resultsPerPage);
            case SearchType.LocalAuthEstablishment:
            case SearchType.LocalAuthEstablishment7Digit:
            case SearchType.LocalAuthEstablishment3Digit:
            case SearchType.LocalAuthEstablishment4Digit:
                return new LaEstabSearchStrategy(_repository, searchTerm, page, resultsPerPage);
            case SearchType.EstablishmentNameOrLocation:
                return new EstablishmentNameOrLocationSearchStrategy(_searchService, searchTerm, page, resultsPerPage);
            default:
                throw new NotSupportedException("Search type not supported.");
        }
    }
}