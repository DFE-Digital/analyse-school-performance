using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch
{
    public interface IEstablishmentSearch : IUseCase<EstablishmentSearchRequest, Result<ScopedSearchResultsPage<EstablishmentListingDTO>>>
    {
    }
}
