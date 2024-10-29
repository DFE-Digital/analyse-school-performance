using ASP.Core.Results;
using ASP.Core.Scoping;

namespace ASP.Core.Establishments.Search;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(IEstablishmentRepository repository, Scope scope, string searchTerm, int page = 1,
        int resultsPerPage = Constants.SearchResultPageSize) : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute()
    {
        return 
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
                    establishment.OfstedRating,
                    establishment.OfstedLastInspectionDate,
                    establishment.Laestab
                )]
            );
    }
}