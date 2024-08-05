using ASP.Core.Establishments;
using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(IEstablishmentRepository repository, string searchTerm, int page = 1,
        int resultsPerPage = Constants.SearchResultPageSize) : base(searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> Execute()
    {
        var result = await _repository.GetEstablishmentDetails(SearchTerm);

        return result.Map(establishment => new SearchResultsPage<EstablishmentDetailsSearchResult>(
            SearchTerm,
            Page,
            ResultsPerPage,
            totalResults: 1,
            [new EstablishmentDetailsSearchResult {
                Urn = establishment.Urn,
                Name = establishment.Name,
                IsPrimary = establishment.IsPrimary,
                IsSecondary = establishment.IsSecondary,
                IsPost16 = establishment.IsPost16,
                Address = establishment.Address,
                OfstedRating = establishment.OfstedRating,
                OfstedLastInspectionDate = establishment.OfstedLastInspectionDate,
                Laestab = establishment.Laestab,
                IsDeleted = establishment.IsDeleted,
                IsVisible = establishment.IsVisible
            }]
        ));
    }
}