using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.DTO;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Establishments.UseCases.GetAllEstablishments;

public interface IGetAllEstablishments : IUseCase<GetAllEstablishmentsRequest, Result<ScopedResultsPage<EstablishmentListingDTO>>>
{
}