using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;

public interface IGetLocalAuthority : IUseCase<GetLocalAuthorityRequest, Result<LocalAuthorityDTO>>
{
}