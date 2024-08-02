using ASP.Core.Establishments;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class UrnLookupStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public UrnLookupStrategy(IEstablishmentRepository repository, string searchTerm, int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize) : base(searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var result = await _repository.GetEstablishmentDetails(SearchTerm);

        return result.Map(establishment => new SearchResultsPage<EstablishmentDetailsSearchResultDTO>(
            SearchTerm,
            Page,
            ResultsPerPage,
            totalResults: 1,
            [establishment.MapToSearchResult()]
        ));
    }
}