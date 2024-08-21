using ASP.Core.Authorization;
using System.Security.Claims;

namespace ASP.Web.FunctionalTests.Services;

public class TestClaimsProvider
{
    private readonly List<Claim> _claims = [];

    public List<Claim> GetClaims()
    {
        return _claims;
    }

    public void SetCategory(string id, string name)
    {
        _claims.Add(new Claim(CustomClaimTypes.CategoryId, id));
        _claims.Add(new Claim(CustomClaimTypes.CategoryName, name));
    }

    public void SetUrn(string urn)
    {
        _claims.Add(new Claim(CustomClaimTypes.UniqueReferenceNumber, urn));
    }

    public void SetUid(string uid)
    {
        _claims.Add(new Claim(CustomClaimTypes.UniqueIdentifier, uid));
    }

    public void SetEstablishmentNumber(string establishmentNumber)
    {
        _claims.Add(new Claim(CustomClaimTypes.EstablishmentNumber, establishmentNumber));
    }

    public void SetOrganisationName(string organisationName)
    {
        _claims.Add(new Claim(CustomClaimTypes.OrganisationName, organisationName));
    }

    public void SetRole(Role role)
    {
        _claims.Add(new Claim(ClaimTypes.Role, role.Code));
    }

    public void ClearClaims()
    {
        _claims.Clear();
    }

    internal void SetName(string firstName, string lastName)
    {
        _claims.Add(new Claim(ClaimTypes.GivenName, firstName));
        _claims.Add(new Claim(ClaimTypes.Surname, lastName));
    }
}
