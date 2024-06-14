using ASP.Core.DTO.Establishment;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public interface IGetEstablishmentDetailsUseCase : IUseCase<GetEstablishmentDetailsUseCaseRequest, Result<EstablishmentDetailsDTO>>
    {
    }
}
