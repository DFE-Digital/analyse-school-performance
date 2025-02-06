using ASP.Core.Results;

namespace ASP.Domain.Establishments
{
    public interface IEstablishmentScopeValidator
    {
        Task<Result<EstablishmentScope>> ValidateScope(EstablishmentScopeType scopeType, string scopeIdentifier);
    }
}
