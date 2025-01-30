using ASP.Core.Results;
using ASP.Domain.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Domain.LocalAuthorities.UseCases.DTO;
using ASP.Domain.LocalAuthorities.UseCases.DTO.Mapper;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;

public class LocalAuthoritySearchSuggestions : ILocalAuthoritySearchSuggestions
{
    private readonly ILocalAuthorityRepository _repository;

    public LocalAuthoritySearchSuggestions(ILocalAuthorityRepository repository)
    {
        _repository = repository ??
                      throw new ArgumentNullException(nameof(repository));
    }

    public Task<Result<LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO>>> HandleRequest(LocalAuthoritySearchSuggestionsRequest request)
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);
        
        var isNumeric = int.TryParse(request.SearchTerm, out var _);

        var suggestions = isNumeric
            ? _repository.LocalAuthoritySearchSuggestionsByLaCode(request.SearchTerm, maxSuggestions)
            : _repository.LocalAuthoritySearchSuggestionsByLaName(request.SearchTerm, maxSuggestions);
        
        return from response in suggestions
        select new LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO> {
            Suggestions = response.MapToLocalAuthorityDTO(),
            MaxSuggestions = maxSuggestions,
            SearchTerm = request.SearchTerm
        };
    }
}