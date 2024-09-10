using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core.Establishments;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestions : IEstablishmentSearchSuggestions
{
    private readonly EstablishmentSearchSuggestionsService _searchService;

    public EstablishmentSearchSuggestions(
        IEstablishmentRepository establishmentRepository,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository
    )
    {
        _searchService = new EstablishmentSearchSuggestionsService(
            establishmentRepository,
            localAuthorityRepository,
            multiAcademyTrustRepository
        );
    }

    public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request
    )
    {
        var maxSuggestions = request.MaxSuggestions ?? Core.Constants.SearchResultMaxSuggestions;

        var scope = new Scope(request.ScopeType, request.ScopeIdentifier);

        return await _searchService.Search(request.SearchTerm, scope, maxSuggestions)
            .Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
            {
                Suggestions = x.Suggestions.MapToListOfSearchSuggestionsResultsDTO(),
                MaxSuggestions = maxSuggestions,
                SearchTerm = request.SearchTerm,
                Scope = request.ScopeType.ToString(),
                ScopeIdentifier = request.ScopeIdentifier
            });
    }
}