using ASP.Core.Results;

namespace ASP.Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope;

public class IsEstablishmentAccessibleInScope : IIsEstablishmentAccessibleInScope
{
    private readonly IEstablishmentRepository _repository;

    public IsEstablishmentAccessibleInScope(IEstablishmentRepository pageContentRepository)
    {
        _repository = pageContentRepository ??
                      throw new ArgumentNullException(nameof(pageContentRepository));
    }

    public Task<Result<IsEstablishmentAccessibleInScopeResponse>> HandleRequest(IsEstablishmentAccessibleInScopeRequest request)
    {
        return
            from isAccessible in _repository.IsEstablishmentVisibleWithinScope(request.Urn,
                    new EstablishmentScope(request.ScopeType, request.ScopeIdentifier.GetValueOrDefault("")))
            select new IsEstablishmentAccessibleInScopeResponse(request.Urn, request.ScopeType.ToString(), request.ScopeIdentifier.GetValueOrDefault(""), isAccessible);
    }
}