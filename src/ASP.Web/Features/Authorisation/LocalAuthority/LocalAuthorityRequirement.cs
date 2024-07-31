using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation.LocalAuthority;

public class LocalAuthorityRequirement : IAuthorizationRequirement
{
    public LocalAuthorityRequirement()
    {
    }
}