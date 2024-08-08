namespace ASP.Core.Establishments.Search;

public interface IEstablishmentSearchStrategyFactory
{
    EstablishmentSearchStrategy CreateStrategy(Scope scope, SearchType searchType, string searchTerm, int page, int resultsPerPage);
}