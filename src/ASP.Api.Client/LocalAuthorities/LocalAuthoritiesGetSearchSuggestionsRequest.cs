namespace ASP.Api.Client.LocalAuthorities;

public record LocalAuthoritiesGetSearchSuggestionsRequest(string SearchTerm, int? MaxSuggestions);