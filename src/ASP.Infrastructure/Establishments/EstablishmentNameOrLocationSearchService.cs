using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;

namespace ASP.Infrastructure.Establishments;

public class EstablishmentNameOrLocationSearchService : ISearchService
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentNameOrLocationSearchService(IEstablishmentRepository establishmentRepository)
    {
        _establishmentRepository = establishmentRepository;
    }

    public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> SearchAsync(string searchTerm, int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize)
    {
        var result = await _establishmentRepository.SearchEstablishmentNameOrLocation(searchTerm, page, resultsPerPage);

        return result;
    }
}