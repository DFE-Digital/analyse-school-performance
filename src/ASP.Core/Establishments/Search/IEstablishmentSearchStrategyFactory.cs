namespace ASP.Core.Establishments.Search;

public interface IEstablishmentSearchStrategyFactory
{
    EstablishmentSearchStrategy CreateStrategy(SearchType searchType, string searchTerm, int page, int resultsPerPage);
}