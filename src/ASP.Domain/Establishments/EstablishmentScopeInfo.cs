using ASP.Core.Optionality;

namespace ASP.Domain.Establishments;

public record EstablishmentScopeInfo(EstablishmentScopeType ScopeType, Optional<string> ScopeId);