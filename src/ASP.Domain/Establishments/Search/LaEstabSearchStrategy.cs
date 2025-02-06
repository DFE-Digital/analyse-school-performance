using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search;

public class LaEstabSearchStrategy : EstablishmentSearchStrategy
{
    private readonly IEstablishmentRepository _repository;

    public LaEstabSearchStrategy(IEstablishmentRepository repository, EstablishmentScope scope,
        string searchTerm, int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize) : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> Execute()
    {
        var searchType = SearchTerm.ClassifySearchType();

        Result<ScopedSearchResultsPage<EstablishmentListing>> result = searchType switch
        {
            SearchType.LocalAuthEstablishment => await _repository.SearchEstablishmentByLaCodeOrEstablishmentNumber(Scope, SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment7Digit => await _repository.SearchEstablishmentByLaestab7DigitCode(Scope, SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment3Digit => await _repository.SearchEstablishmentByLaCode(Scope, SearchTerm, Page, ResultsPerPage),
            SearchType.LocalAuthEstablishment4Digit => await _repository.SearchEstablishmentByEstablishmentNumber(Scope, SearchTerm, Page, ResultsPerPage),
            _ => throw new InvalidOperationException($@"Invalid SearchType: ""{searchType}"".")
        };

        return result;
    }
}