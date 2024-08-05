using ASP.Core.Extensions;
using ASP.Core.Results;

namespace ASP.Core.Establishments.Search;

public class LaEstabSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public LaEstabSearchStrategy(IEstablishmentRepository repository,
        string searchTerm, int page = 1,
        int resultsPerPage = Constants.SearchResultPageSize) : base(searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<SearchResultsPage<EstablishmentDetailsSearchResult>>> Execute()
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

        return result;
    }
}