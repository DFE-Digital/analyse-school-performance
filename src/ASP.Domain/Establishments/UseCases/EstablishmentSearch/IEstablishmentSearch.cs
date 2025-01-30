using ASP.Domain.Establishments.Search;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Domain.Establishments.UseCases.DTO;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearch
{
    public interface IEstablishmentSearch : IUseCase<EstablishmentSearchRequest, Result<ScopedSearchResultsPage<EstablishmentListingDTO>>>
    {
    }
}
