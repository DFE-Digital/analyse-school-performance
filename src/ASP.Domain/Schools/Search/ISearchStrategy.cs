using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Schools.Search;

public interface ISearchStrategy
{
    Task<Result<ResultsPage<School>>> Execute();
}