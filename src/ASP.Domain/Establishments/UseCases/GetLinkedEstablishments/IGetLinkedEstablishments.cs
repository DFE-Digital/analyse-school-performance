using ASP.Core.Results;
using ASP.Domain.Establishments.UseCases.DTO;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

public interface IGetLinkedEstablishments : IUseCase<GetLinkedEstablishmentsRequest, Result<LinkedEstablishmentsResponseDTO>>
{
}