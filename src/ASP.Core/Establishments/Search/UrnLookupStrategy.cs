using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(IEstablishmentRepository repository, Scope scope, string searchTerm, int page = 1,
        int resultsPerPage = Constants.SearchResultPageSize) : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentListItem>>> Execute()
    {
        var result = await _repository.GetEstablishmentDetails(SearchTerm);

        return result.Map(establishment => new SearchResultsPage<EstablishmentListItem>(
            SearchTerm,
            Scope.ScopeType.ToString(),
            Scope.ScopeIdentifier,
            Page,
            ResultsPerPage,
            totalResults: 1,
            [new EstablishmentListItem(
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
        ));
    }
}