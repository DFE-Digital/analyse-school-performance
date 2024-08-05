using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public interface IGetEstablishmentDetails : IUseCase<GetEstablishmentDetailsRequest, Result<EstablishmentDetailsDTO>>
    {
    }
}
