using ASP.Core.Results;
using ASP.Core.Search.Suggestions;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public interface IEstablishmentSearchSuggestionsUseCase : IUseCase<EstablishmentSearchSuggestionsUseCaseRequest,
    Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>>
{
}