using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorization.LocalAuthority;

public class LocalAuthorityRequirement : IAuthorizationRequirement
{
    public LocalAuthorityRequirement()
    {
    }
}