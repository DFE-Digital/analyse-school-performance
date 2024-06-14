using ASP.Core.Establishments;
using ASP.Core.Helpers;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class LaEstabSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;
    
    public LaEstabSearchStrategy(string searchTerm, int page, IEstablishmentRepository repository) : base(searchTerm, page)
    {
        _repository = repository;
    }
    
    public override async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var (skip, take) = PageHelper.ConstructPagingRequest(Page);
        var result = await _repository.SearchLocalAuthEstablishment(SearchTerm, skip, take);

        return result.Map(x => new SearchResult<EstablishmentDetailsSearchResultDTO>()
        {
            Results = x.Results.MapToListOfSearchResultsDTO(),
            ResultCount = x.ResultCount,
            TotalCount = x.TotalCount
        });
    }
}