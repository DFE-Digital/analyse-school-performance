using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Core.Pagination;
using ASP.Domain.Establishments.SearchSuggestions;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public interface IEstablishmentSearchSuggestions : IUseCase<EstablishmentSearchSuggestionsRequest,
    Result<ScopedSearchSuggestionsList<EstablishmentSuggestion>>>
{
}