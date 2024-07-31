using ASP.Web.Features.Authorisation.LocalAuthority;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation;

public static class Policy
{
    public const string LocalAuthorityAccessPolicy = "LocalAuthorityAccessPolicy";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(LocalAuthorityAccessPolicy, policy =>
            policy.Requirements.Add(new LocalAuthorityRequirement()));
    }
}