using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.Establishments.SearchSuggestions;

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

    public Task<Result<ScopedSearchSuggestionsList<EstablishmentSuggestion>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request
    )
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);

        return
            from scope in _scopeValidator.ValidateScope(request.Scope)
            from response in _searchService.Search(request.SearchTerm, scope, maxSuggestions)
            select response;
    }
}