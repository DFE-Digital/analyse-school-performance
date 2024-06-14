using ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;
using ASP.Core.Establishments;
using ASP.Core.Extensions;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

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

    public EstablishmentSearchStrategy CreateStrategy(string searchTerm, int page)
    {
        var searchType = searchTerm.ClassifySearchType();
        
        switch (searchType)
        {
            case SearchType.Urn:
            return new UrnLookupStrategy(searchTerm, _repository);
            case SearchType.LocalAuthEstablishment:
            case SearchType.LocalAuthEstablishment7Digit:
            case SearchType.LocalAuthEstablishment3Digit:
            case SearchType.LocalAuthEstablishment4Digit:
                return new LaEstabSearchStrategy(searchTerm, page, _repository);
            case SearchType.EstablishmentNameOrLocation:
                return new EstablishmentNameOrLocationSearchStrategy(searchTerm, page, _searchService);
            default:
                throw new NotSupportedException("Search type not supported.");
        }
    }
}