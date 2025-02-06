using ASP.Core.Optionality;

namespace ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope;

public record IsEstablishmentAccessibleInScopeRequest(string Urn, EstablishmentScopeType ScopeType, Optional<string> ScopeIdentifier);