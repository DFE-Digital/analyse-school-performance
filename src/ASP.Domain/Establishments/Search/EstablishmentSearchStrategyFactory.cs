namespace ASP.Domain.Establishments.Search;

public class EstablishmentSearchStrategyFactory
{
    private readonly IEstablishmentRepository _repository;

    public EstablishmentSearchStrategyFactory(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public EstablishmentSearchStrategy CreateStrategy(EstablishmentScope scope, SearchType searchType, string searchTerm, int page, int resultsPerPage)
    {
        switch (searchType)
        {
            case SearchType.Urn:
                return new UrnLookupStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            case SearchType.LocalAuthEstablishment:
            case SearchType.LocalAuthEstablishment7Digit:
            case SearchType.LocalAuthEstablishment3Digit:
            case SearchType.LocalAuthEstablishment4Digit:
                return new LaEstabSearchStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            case SearchType.EstablishmentNameOrLocation:
                return new EstablishmentNameOrLocationSearchStrategy(_repository, scope, searchTerm, page, resultsPerPage);
            default:
                throw new NotSupportedException("Search type not supported.");
        }
    }
}