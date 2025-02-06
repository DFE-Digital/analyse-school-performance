using ASP.Core.Pagination;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;

public interface IGetAllLocalAuthorities : IUseCase<GetAllLocalAuthoritiesRequest,
    Result<ResultsPage<LocalAuthority>>>
{
}