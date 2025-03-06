using ASP.Core.Results;

namespace ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;

public class LocalAuthoritySearchSuggestions : ILocalAuthoritySearchSuggestions
{
    private readonly ILocalAuthorityRepository _repository;

    public LocalAuthoritySearchSuggestions(ILocalAuthorityRepository repository)
    {
        _repository = repository ??
                      throw new ArgumentNullException(nameof(repository));
    }

    public Task<Result<List<LocalAuthority>>> HandleRequest(LocalAuthoritySearchSuggestionsRequest request)
    {
        var maxSuggestions = request.MaxSuggestions.GetValueOrDefault(Core.Constants.SearchResultMaxSuggestions);
        
        var isNumeric = int.TryParse(request.SearchTerm, out var _);

        var suggestions = isNumeric
            ? _repository.SearchSuggestions(new PartialCodeSearchCriteria(request.SearchTerm), maxSuggestions)
            : _repository.SearchSuggestions(new NameSearchCriteria(request.SearchTerm), maxSuggestions);

        return suggestions;
    }
}