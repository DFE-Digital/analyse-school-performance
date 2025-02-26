using ASP.Core.Optionality;
using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Schools.Search;

public abstract class SearchStrategy : ISearchStrategy
{
    protected Optional<SchoolAccessScope> Scope { get; }
    protected string SearchTerm { get; }
    protected int Page { get; }
    protected int ResultsPerPage { get; }

    protected SearchStrategy(Optional<SchoolAccessScope> scope, string searchTerm, int page, int resultsPerPage)
    {
        Scope = scope;
        SearchTerm = searchTerm;
        Page = page;
        ResultsPerPage = resultsPerPage;
    }

    public abstract Task<Result<ResultsPage<School>>> Execute();
}