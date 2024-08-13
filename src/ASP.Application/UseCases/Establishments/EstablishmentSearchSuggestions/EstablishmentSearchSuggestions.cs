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
    private readonly IEstablishmentRepository _repository;
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

    public EstablishmentSearchSuggestions(IEstablishmentRepository repository,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
        _multiAcademyTrustRepository = multiAcademyTrustRepository;
        _repository = repository;
    }

    public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsRequest request)
    {
        var maxSuggestions = request.MaxSuggestions ?? Core.Constants.SearchResultMaxSuggestions;

        var scope = new Scope(request.ScopeType, request.ScopeIdentifier);

        return await scope.Validate(_localAuthorityRepository, _multiAcademyTrustRepository)
            .Then(async t => await _repository.EstablishmentSearchSuggestions(scope, request.SearchTerm,
                maxSuggestions)).Map(x =>
                new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
                {
                    Suggestions = x.Suggestions.MapToListOfSearchSuggestionsResultsDTO(),
                    MaxSuggestions = maxSuggestions,
                    SearchTerm = request.SearchTerm,
                    Scope = request.ScopeType.ToString(),
                    ScopeIdentifier = request.ScopeIdentifier
                });
    }
}