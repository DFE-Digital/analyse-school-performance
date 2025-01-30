using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentNameOrLocationSearchStrategy(
        IEstablishmentRepository establishmentRepository,
        EstablishmentScope scope, 
        string searchTerm,
        int page, 
        int resultsPerPage
    ) : base(scope, searchTerm, page, resultsPerPage)
    {
        _establishmentRepository = establishmentRepository;
    }

    public override async Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute()
    {
        var results = await _establishmentRepository.SearchEstablishmentNameOrLocation(Scope, SearchTerm, Page, ResultsPerPage);

        return results;
    }
}