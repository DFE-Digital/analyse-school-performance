using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;

public interface IGetLocalAuthority : IUseCase<GetLocalAuthorityRequest, Result<LocalAuthority>>
{
}