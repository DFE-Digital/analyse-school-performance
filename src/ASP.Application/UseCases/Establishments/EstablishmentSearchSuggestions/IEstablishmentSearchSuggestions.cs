using ASP.Core.Results;
using ASP.Core.Search.Suggestions;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public interface IEstablishmentSearchSuggestions : IUseCase<EstablishmentSearchSuggestionsRequest,
    Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>>
{
}