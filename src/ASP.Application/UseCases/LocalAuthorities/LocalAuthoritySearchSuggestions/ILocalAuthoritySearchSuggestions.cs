using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Core.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearchSuggestions;

public interface ILocalAuthoritySearchSuggestions : IUseCase<LocalAuthoritySearchSuggestionsRequest,
    Result<LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO>>>
{
}