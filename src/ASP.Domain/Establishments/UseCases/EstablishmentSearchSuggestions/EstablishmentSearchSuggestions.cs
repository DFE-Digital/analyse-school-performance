using ASP.Core.Results;
using ASP.Domain.Establishments.SearchSuggestions;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.DTO.Mapper;

namespace ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestions : IEstablishmentSearchSuggestions
{
    private readonly IEstablishmentScopeValidator _scopeValidator;
    private readonly EstablishmentSearchSuggestionsService _searchService;

    public EstablishmentSearchSuggestions(
        IEstablishmentRepository establishmentRepository,
        IEstablishmentScopeValidator scopeValidator)
    {

        _scopeValidator = scopeValidator;
        _searchService = new EstablishmentSearchSuggestionsService(establishmentRepository);
    }

    public Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request
    )
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return
            from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier)
            from response in _searchService.Search(request.SearchTerm, scope, maxSuggestions)
            select new SearchSuggestionsResult<EstablishmentSuggestionDTO> {
                Suggestions = response.Suggestions.MapToListOfEstablishmentSuggestionDTO(),
                MaxSuggestions = maxSuggestions,
                SearchTerm = request.SearchTerm,
                Scope = request.ScopeType.ToString(),
                ScopeIdentifier = scopeIdentifier
            };
    }
}