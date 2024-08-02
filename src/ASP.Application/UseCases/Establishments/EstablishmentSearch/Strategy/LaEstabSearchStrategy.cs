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

    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResultDTO>>> Execute()
    {
        var searchType = SearchTerm.ClassifySearchType();

        Result<SearchResultsPage<EstablishmentDetailsSearchResult>> result = searchType switch
        {
            SearchType.LocalAuthEstablishment => await _repository.SearchEstablishmentByLaCodeOrEstablishmentNumber(SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment7Digit => await _repository.SearchEstablishmentByLocalAuthEstablishment7DigitCode(SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment3Digit => await _repository.SearchEstablishmentByLaCode(SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment4Digit => await _repository.SearchEstablishmentByEstablishmentNumber(SearchTerm, Page, ResultsPerPage),
            _ => throw new InvalidOperationException($@"Invalid SearchType: ""{searchType}"".")
        };

        return result.Map(results => results.Map(r => r.MapToSearchResultDTO()));
    }
}