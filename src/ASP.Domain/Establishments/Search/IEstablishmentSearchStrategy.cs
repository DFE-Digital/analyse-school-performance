using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Domain.Establishments.Search;

public interface IEstablishmentSearchStrategy
{
    Task<Result<ResultsPage<EstablishmentListing>>> Execute();
}