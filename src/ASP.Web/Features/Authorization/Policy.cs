using ASP.Core.Authorization;
using ASP.Web.Features.Authorization.LocalAuthority;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorization;

public static class Policy
{
    public const string LocalAuthorityAccessPolicy = "LocalAuthorityAccessPolicy";
    public const string Any = "Any";
    public const string AccessToMyLa = "AccessToMyLa";
    public const string AccessToSearch = "AccessToSearch";
    public const string AccessToMySchool = "AccessToMySchool";
    public const string AccessToMySchools = "AccessToMySchools";
    public const string AccessToEditPages = "AccessToEditPages";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Any, policy => policy.RequireAssertion(context => context.User.HasRole(r => r.IsAny)));

        options.AddPolicy(AccessToSearch, policy => policy.RequireAssertion(context => context.User.HasRole(r => r.HasAccessToSearch)));

        options.AddPolicy(AccessToMySchool, policy => policy.RequireAssertion(context => context.User.HasRole(r => r.HasAccessToMySchool)));

        options.AddPolicy(AccessToMySchools, policy => policy.RequireAssertion(context => context.User.HasRole(r => r.HasAccessToMySchools)));

        options.AddPolicy(AccessToMyLa, policy => policy.RequireAssertion(context => context.User.HasRole(r => r.HasAccessToMyLa)));

        options.AddPolicy(LocalAuthorityAccessPolicy, policy =>
            policy.Requirements.Add(new LocalAuthorityRequirement()));

        options.AddPolicy(AccessToEditPages, policy =>
            policy.RequireRole(Role.AccessToEditPages));

        // Add fallback policy for all routes, requiring the user to be authenticated
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    }
}