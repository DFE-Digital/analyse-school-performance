using ASP.Domain.Establishments.SearchSuggestions;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;
using ASP.Domain.Establishments.UseCases.DTO;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public interface IEstablishmentSearchSuggestions : IUseCase<EstablishmentSearchSuggestionsRequest,
    Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>>
{
}