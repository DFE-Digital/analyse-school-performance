using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Domain.Establishments
{
    public interface IEstablishmentScopeValidator
    {
        Task<Result<EstablishmentScope>> ValidateScope(EstablishmentScopeType scopeType, string scopeIdentifier);
        Task<Result<EstablishmentScope>> ValidateScope(Optional<EstablishmentScopeInfo> scope);
    }
}
