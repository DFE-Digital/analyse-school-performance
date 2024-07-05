using ASP.Core.Establishments;
using ASP.Core.Extensions;
using ASP.Core.Mapper.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Suggestions;

namespace ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;

public class EstablishmentSearchSuggestionsUseCase : IEstablishmentSearchSuggestionsUseCase
{
    private readonly IEstablishmentRepository _repository;

    public EstablishmentSearchSuggestionsUseCase(IEstablishmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>> HandleRequest(
        EstablishmentSearchSuggestionsUseCaseRequest request)
    {
        var searchType = request.SearchTerm.ClassifySearchType();

        if (searchType == SearchType.Urn)
        {
            var result = await _repository.GetEstablishmentDetails(request.SearchTerm);

            return result.Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
            {
                Suggestions = new EstablishmentSearchSuggestionsResultDTO[]
                {
                    x.MapToSearchSuggestionsResult()
                },
                MaxSuggestions = request.MaxSuggestions,
                SearchTerm = request.SearchTerm,
            });
        }

        var results = await _repository.EstablishmentSearchSuggestions(request.SearchTerm, request.MaxSuggestions);

        return results.Map(x => new SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>()
        {
            Suggestions = x.Suggestions.MapToListOfSearchSuggestionsResultsDTO(),
            MaxSuggestions = request.MaxSuggestions,
            SearchTerm = request.SearchTerm,
        });
    }
}