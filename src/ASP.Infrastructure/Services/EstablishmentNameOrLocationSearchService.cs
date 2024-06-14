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

    public async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> SearchAsync(string searchTerm, int page)
    {
        var (skip, take) = PageHelper.ConstructPagingRequest(page);
        
        var result = await _establishmentRepository.SearchEstablishmentNameOrLocation(searchTerm, skip, take);

        return result.Map(x => new SearchResult<EstablishmentDetailsSearchResultDTO>()
        {
            Results = x.Results.MapToListOfSearchResultsDTO(),
            ResultCount = x.ResultCount,
            TotalCount = x.TotalCount
        });
    }
}