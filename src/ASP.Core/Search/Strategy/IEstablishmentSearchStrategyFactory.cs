namespace ASP.Core.Search.Strategy;

public interface IEstablishmentSearchStrategyFactory
{
    EstablishmentSearchStrategy CreateStrategy(SearchType searchType, string searchTerm, int page, int resultsPerPage);
}