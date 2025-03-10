using ASP.Core.Optionality;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search;

public class UrnLookupStrategy : SearchStrategy
{
    private readonly ISchoolRepository _repository;

    public UrnLookupStrategy(
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
        return 
            from urn in SchoolUrn.Parse(SearchTerm)
            from school in _repository.GetWithLinkedSchools(urn)
                .MapErrorIf(e => e is NotFoundError, Error.NotFound($@"There were no matches for ""{SearchTerm}""."))
                // TODO: fixed bug here, need tests
                .ErrorIf(school => !school.IsAccessibleInScope(Scope), Error.NotFound($@"There were no matches for ""{SearchTerm}""."))
            select new ResultsPage<School>(
                Page,
                ResultsPerPage,
                totalResults: 1,
                [school])
        ;
    }
}