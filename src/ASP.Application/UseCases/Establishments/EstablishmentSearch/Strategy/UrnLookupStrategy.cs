using ASP.Core.Establishments;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(string searchTerm, IEstablishmentRepository repository) : base(searchTerm)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var results = await _repository.GetEstablishmentDetails(SearchTerm).Map(x => new SearchResult<EstablishmentDetailsSearchResultDTO>
        {
            Results = new EstablishmentDetailsSearchResultDTO[]
            {
                x.MapToSearchResult()
            },
            ResultCount = 1,
            TotalCount = 1
        });

        return results;
    }
}