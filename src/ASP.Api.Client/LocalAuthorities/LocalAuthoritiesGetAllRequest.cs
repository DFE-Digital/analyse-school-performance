namespace ASP.Api.Client.LocalAuthorities;

public record LocalAuthoritiesGetAllRequest(string? SearchTerm, int? Page, int? ResultsPerPage);