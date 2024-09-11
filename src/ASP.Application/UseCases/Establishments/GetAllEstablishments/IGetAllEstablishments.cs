using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core.Results;
using ASP.Core.Scope;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.GetAllEstablishments;

public interface IGetAllEstablishments : IUseCase<GetAllEstablishmentsRequest, Result<ScopedResultsPage<EstablishmentListingDTO>>>
{
}