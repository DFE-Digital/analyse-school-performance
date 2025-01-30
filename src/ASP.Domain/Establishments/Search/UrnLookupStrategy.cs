using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(IEstablishmentRepository repository, EstablishmentScope scope, string searchTerm, int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize) : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute()
    {
        return
            from isAllowed in _repository.IsEstablishmentVisibleWithinScope(SearchTerm, Scope)
                .ErrorIf(exists => !exists, Error.NotFound($@"there were no matches for ""{SearchTerm}""."))
            from establishment in _repository.GetEstablishmentDetails(SearchTerm)
            select new ScopedSearchResultsPage<EstablishmentListing>(
                SearchTerm,
                Scope.ScopeType.ToString(),
                Scope.ScopeIdentifier,
                Page,
                ResultsPerPage,
                totalResults: 1,
                [new EstablishmentListing(
                    establishment.Urn,
                    establishment.Name,
                    establishment.IsPrimary,
                    establishment.IsSecondary,
                    establishment.IsPost16,
                    establishment.Address,
                    establishment.Laestab
                )]
            );
    }
}