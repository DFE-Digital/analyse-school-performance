using ASP.Core.Pagination;

namespace ASP.Api.Functions.LocalAuthorities;

public static class DomainMappingExtensions
{
    public static Client.LookupValueWithCode ForApiClient(this Domain.LocalAuthorities.LocalAuthority localAuthority)
    {
        return new Client.LookupValueWithCode(
            localAuthority.Code,
            localAuthority.Name);
    }

    public static ResultsPage<Client.LookupValueWithCode>? ForApiClient(this ResultsPage<Domain.LocalAuthorities.LocalAuthority>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return response.Map(r => r.ForApiClient());
    }

    public static SearchResultsPage<Client.LookupValueWithCode>? ForApiClient(this SearchResultsPage<Domain.LocalAuthorities.LocalAuthority>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return response.Map(r => r.ForApiClient());
    }

    public static SearchSuggestionsList<Client.LookupValueWithCode>? ForApiClient(this SearchSuggestionsList<Domain.LocalAuthorities.LocalAuthority>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return new SearchSuggestionsList<Client.LookupValueWithCode> {
            Suggestions = response.Suggestions.ForApiClient(),
            MaxSuggestions = response.MaxSuggestions,
            SearchTerm = response.SearchTerm,
        };
    }

    public static List<Client.LookupValueWithCode> ForApiClient(
        this IEnumerable<Domain.LocalAuthorities.LocalAuthority> list)
    {
        return list.Select(ForApiClient).ToList();
    }
}