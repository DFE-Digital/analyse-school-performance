using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Core.Pagination;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch
{
    public interface IEstablishmentSearch : IUseCase<EstablishmentSearchRequest, Result<ScopedSearchResultsPage<EstablishmentListing>>>
    {
    }
}
