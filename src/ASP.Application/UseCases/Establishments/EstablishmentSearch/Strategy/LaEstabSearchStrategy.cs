using ASP.Core.Establishments;
using ASP.Core.Extensions;
using ASP.Core.Helpers;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Strategy;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch.Strategy;

public class LaEstabSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public LaEstabSearchStrategy(IEstablishmentRepository repository,
        string searchTerm, int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize) : base(searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var searchType = SearchTerm.ClassifySearchType();
        var (skip, take) = PageHelper.ConstructPagingRequest(Page, ResultsPerPage);

        Result<SearchResult<EstablishmentDetailsSearchResult>>? result = searchType switch
        {
            SearchType.LocalAuthEstablishment => await _repository.SearchEstablishmentByLaCodeOrEstablishmentNumber(
                SearchTerm, skip, take),
            SearchType.LocalAuthEstablishment7Digit => await _repository
                .SearchEstablishmentByLocalAuthEstablishment7DigitCode(SearchTerm, skip, take),
            SearchType.LocalAuthEstablishment3Digit => await _repository.SearchEstablishmentByLaCode(SearchTerm, skip,
                take),
            SearchType.LocalAuthEstablishment4Digit => await _repository.SearchEstablishmentByEstablishmentNumber(
                SearchTerm, skip, take),
            _ => null
        };

        return result!.Map(x => new SearchResult<EstablishmentDetailsSearchResultDTO>()
        {
            Results = x.Results.MapToListOfSearchResultsDTO(),
            ResultsPerPage = x.ResultsPerPage,
            TotalResults = x.TotalResults,
            SearchTerm = SearchTerm,
            Page = Page
        });
    }
}