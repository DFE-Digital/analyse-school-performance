namespace ASP.Api.Client.Establishments;

public record GetAllEstablishmentsRequest(EstablishmentScopeType ScopeType, string? ScopeIdentifier, int? Page, int? ResultsPerPage);