using ASP.Core.Optionality;

namespace ASP.Core.Scoping;

public record ScopeInfo(ScopeType ScopeType, Optional<string> ScopeId);