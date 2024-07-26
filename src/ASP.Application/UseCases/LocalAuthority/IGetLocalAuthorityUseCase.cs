using ASP.Core.DTO.LocalAuthority;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.LocalAuthority;

public interface IGetLocalAuthorityUseCase : IUseCase<GetLocalAuthorityRequest, Result<LocalAuthorityDTO>>
{
}