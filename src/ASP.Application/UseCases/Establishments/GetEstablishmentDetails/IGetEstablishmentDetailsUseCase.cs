using ASP.Core.Establishments;
using ASP.Core.Results;
using ASP.Core.Templating;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.GetEstablishmentDetails
{
    public interface IGetEstablishmentDetailsUseCase : IUseCase<GetEstablishmentDetailsUseCaseRequest, Result<EstablishmentDetails>>
    {
    }
}
