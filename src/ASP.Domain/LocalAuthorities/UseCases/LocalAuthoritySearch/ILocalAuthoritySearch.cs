using ASP.Core.Pagination;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;

public interface ILocalAuthoritySearch : IUseCase<LocalAuthoritySearchRequest,
    Result<SearchResultsPage<LocalAuthority>>>
{
}