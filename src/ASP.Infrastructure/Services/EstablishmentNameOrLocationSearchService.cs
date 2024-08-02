using ASP.Core.Establishments;
using ASP.Core.Helpers;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;

namespace ASP.Infrastructure.Services;

public class EstablishmentNameOrLocationSearchService : ISearchService
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentNameOrLocationSearchService(IEstablishmentRepository establishmentRepository)
    {
        _establishmentRepository = establishmentRepository;
    }

    public async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> SearchAsync(string searchTerm, int page = 1, 
        int resultsPerPage = Core.Constants.SearchResultPageSize)
    {
        var result = await _establishmentRepository.SearchEstablishmentNameOrLocation(searchTerm, page, resultsPerPage);

        return result.Map(results => results.Map(r => r.MapToSearchResultDTO()));
    }
}