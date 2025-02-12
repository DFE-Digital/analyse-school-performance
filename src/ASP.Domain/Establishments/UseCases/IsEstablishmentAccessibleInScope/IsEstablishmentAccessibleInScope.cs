using ASP.Core.Results;

namespace ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope;

public class IsEstablishmentAccessibleInScope : IIsEstablishmentAccessibleInScope
{
    private readonly IEstablishmentRepository _repository;
    private readonly IEstablishmentScopeValidator _scopeValidator;

    public IsEstablishmentAccessibleInScope(IEstablishmentRepository pageContentRepository,
        IEstablishmentScopeValidator scopeValidator)
    {
        _repository = pageContentRepository ??
                      throw new ArgumentNullException(nameof(pageContentRepository));
        _scopeValidator = scopeValidator ?? throw new ArgumentNullException(nameof(scopeValidator));
    }

    public Task<Result<IsEstablishmentAccessibleInScopeResponse>> HandleRequest(
        IsEstablishmentAccessibleInScopeRequest request)
    {
        var scopeIdentifier = request.ScopeIdentifier.GetValueOrDefault("");

        return
            from scope in _scopeValidator.ValidateScope(request.ScopeType, scopeIdentifier)
            from isAccessible in _repository.IsEstablishmentVisibleWithinScope(request.Urn,
                new EstablishmentScope(request.ScopeType, scopeIdentifier))
            from linkedEstablishments in _repository.GetLinkedEstablishments(request.Urn)
            select new IsEstablishmentAccessibleInScopeResponse(request.Urn, request.ScopeType.ToString(),
                scopeIdentifier, isAccessible,
                new EstablishmentAccess(linkedEstablishments).IsAccessibleViaLinkedSchools(scope));
    }
}