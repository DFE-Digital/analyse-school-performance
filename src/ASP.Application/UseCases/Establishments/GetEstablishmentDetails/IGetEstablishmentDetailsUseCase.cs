using ASP.Core.Establishments;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.GetEstablishmentDetails
{
    public interface IGetEstablishmentDetailsUseCase : IUseCase<GetEstablishmentDetailsUseCaseRequest, Result<EstablishmentDetails>>
    {
    }
}
