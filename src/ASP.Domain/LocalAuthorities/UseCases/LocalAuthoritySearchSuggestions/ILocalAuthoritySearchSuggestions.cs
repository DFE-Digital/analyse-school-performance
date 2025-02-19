using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;

public interface ILocalAuthoritySearchSuggestions : IUseCase<LocalAuthoritySearchSuggestionsRequest,
    Result<List<LocalAuthority>>>
{
}