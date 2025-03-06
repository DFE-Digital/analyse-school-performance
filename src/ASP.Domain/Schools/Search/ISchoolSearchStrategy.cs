using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Schools.Search;

public interface ISchoolSearchStrategy
{
    Task<Result<ResultsPage<School>>> Execute();
}