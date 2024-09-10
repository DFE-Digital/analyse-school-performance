using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class EstablishmentNameOrLocationSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentNameOrLocationSearchStrategy(
        IEstablishmentRepository establishmentRepository, 
        Scope scope, 
        string searchTerm,
        int page, 
        int resultsPerPage
    ) : base(scope, searchTerm, page, resultsPerPage)
    {
        _establishmentRepository = establishmentRepository;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentListItem>>> Execute()
    {
        var results = await _establishmentRepository.SearchEstablishmentNameOrLocation(Scope, SearchTerm, Page, ResultsPerPage);

        return results;
    }
}