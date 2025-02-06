namespace ASP.Api.Client.LocalAuthorities;

public record LocalAuthoritySearchSuggestionsRequest(string SearchTerm, int? MaxSuggestions);