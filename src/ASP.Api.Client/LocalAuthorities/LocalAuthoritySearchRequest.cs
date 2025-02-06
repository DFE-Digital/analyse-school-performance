namespace ASP.Api.Client.LocalAuthorities;

public record LocalAuthoritySearchRequest(string SearchTerm, int? Page, int? ResultsPerPage);