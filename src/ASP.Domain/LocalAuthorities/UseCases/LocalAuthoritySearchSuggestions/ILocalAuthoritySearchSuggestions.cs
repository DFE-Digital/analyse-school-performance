using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Core.Pagination;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;

public interface ILocalAuthoritySearchSuggestions : IUseCase<LocalAuthoritySearchSuggestionsRequest,
    Result<SearchSuggestionsList<LocalAuthority>>>
{
}