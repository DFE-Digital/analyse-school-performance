namespace ASP.Core.Search.Strategy;

public interface IEstablishmentSearchStrategyFactory
{
    EstablishmentSearchStrategy CreateStrategy(string searchTerm, int page);
}