using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.DTO.Mapper;
using ASP.Core.Establishments;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Scoping;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestions : IEstablishmentSearchSuggestions
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;
    private readonly EstablishmentSearchSuggestionsService _searchService;

    public EstablishmentSearchSuggestions(
        IEstablishmentRepository establishmentRepository,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository
    )
    {
        _localAuthorityRepository = localAuthorityRepository;
        _multiAcademyTrustRepository = multiAcademyTrustRepository;

        _searchService = new EstablishmentSearchSuggestionsService(establishmentRepository);
    }

    public async Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request
    )
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return await Scope.Validate(request.ScopeType, scopeIdentifier, _localAuthorityRepository, _multiAcademyTrustRepository)
            .Then(scope => _searchService.Search(request.SearchTerm, scope, maxSuggestions)
            .Map(x => new SearchSuggestionsResult<EstablishmentSuggestionDTO>()
            {
                Suggestions = x.Suggestions.MapToListOfEstablishmentSuggestionDTO(),
                MaxSuggestions = maxSuggestions,
                SearchTerm = request.SearchTerm,
                Scope = request.ScopeType.ToString(),
                ScopeIdentifier = scopeIdentifier
            }));
    }
}