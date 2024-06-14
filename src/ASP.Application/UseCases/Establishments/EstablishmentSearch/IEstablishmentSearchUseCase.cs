using ASP.Core.Results;
using ASP.Core.Search;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearch
{
    public interface IEstablishmentSearchUseCase : IUseCase<EstablishmentSearchUseCaseRequest, Result<SearchResult<EstablishmentDetailsSearchResultDTO>>>
    {
    }
}
