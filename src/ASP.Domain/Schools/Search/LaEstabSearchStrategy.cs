using ASP.Core.Optionality;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search;

public class LaEstabSearchStrategy : SearchStrategy
{
    private readonly ISchoolRepository _repository;

    public LaEstabSearchStrategy(
        ISchoolRepository repository,
        Optional<SchoolAccessScope> scope,
        string searchTerm,
        int page = 1,
        int resultsPerPage = Core.Constants.SearchResultPageSize)
        : base(scope, searchTerm, page, resultsPerPage)
    {
        _repository = repository;
    }

    public override Task<Result<ResultsPage<School>>> Execute()
    {
        return from criteria in FullLACodeOrEstabCodeSearchCriteria.Parse(SearchTerm)
               from results in _repository.SearchByCriteria(criteria, Scope, Page, ResultsPerPage)
               select results;
    }
}