using ASP.Core.Results;
using System.Security.Claims;

namespace ASP.Domain.Establishments
{
    public interface IEstablishmentScopeValidator
    {
        Task<Result<EstablishmentScope>> ValidateScope(EstablishmentScopeType scopeType, string scopeIdentifier);
        Task<Result<EstablishmentScopeInfo>> GetScopeInfoForRole(ClaimsPrincipal user);
    }
}
