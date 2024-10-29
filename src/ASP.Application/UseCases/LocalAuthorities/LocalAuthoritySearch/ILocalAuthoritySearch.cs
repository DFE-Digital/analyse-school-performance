using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Core.Results;
using ASP.Core.Utilities;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearch;

public interface ILocalAuthoritySearch : IUseCase<LocalAuthoritySearchRequest,
    Result<SearchResultsPage<LocalAuthorityDTO>>>
{
}