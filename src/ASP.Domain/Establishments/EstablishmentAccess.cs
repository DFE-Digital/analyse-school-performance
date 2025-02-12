namespace ASP.Domain.Establishments;

public class EstablishmentAccess
{
    public EstablishmentAccess(IReadOnlyCollection<EstablishmentDetails> establishments)
    {
        Establishments = establishments;
    }

    public IReadOnlyCollection<EstablishmentDetails> Establishments { get; }

    public bool IsAccessibleViaLinkedSchools(EstablishmentScope scope)
    {
        if (scope.ScopeType is EstablishmentScopeType.All or EstablishmentScopeType.LA)
        {
            return false;
        }

        if (!Establishments.Any())
        {
            return false;
        }

        return scope.ScopeType switch
        {
            EstablishmentScopeType.MAT => Establishments.Any(
                establishment => establishment.MultiAcademyTrust?.Id.ToString() == scope.ScopeIdentifier),

            EstablishmentScopeType.Diocese => Establishments.Any(
                establishment => establishment.Diocese?.Name == scope.ScopeIdentifier),

            _ => false
        };
    }
}