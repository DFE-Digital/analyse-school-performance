using ASP.Core.Optionality;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search;

public class NameOrLocationSearchStrategy : SearchStrategy
{
    private readonly ISchoolRepository _repository;

    public NameOrLocationSearchStrategy(
        ISchoolRepository repository,
        Optional<SchoolAccessScope> scope,
        string searchTerm,
        int page,
        int resultsPerPage)
        : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override async Task<Result<ResultsPage<School>>> Execute()
    {
        var results = await _repository.SearchByCriteria(new NameOrAddressSearchCriteria(SearchTerm), Scope, Page, ResultsPerPage);

        return results;
    }
}