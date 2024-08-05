using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.CleanArchitecture.CleanArchitecture.Application.UseCase;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public interface IEstablishmentSearchSuggestions : IUseCase<EstablishmentSearchSuggestionsRequest,
    Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>>
{
}