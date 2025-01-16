using ASP.Core.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorization;

public static class Policy
{
    public const string Any = "Any";
    public const string AccessToSearch = "AccessToSearch";
    public const string AccessToMyLocalAuthority = "AccessToMyLocalAuthority";
    public const string AccessToMySchool = "AccessToMySchool";
    public const string AccessToMySchools = "AccessToMySchools";
    public const string AccessToMyLaSchools = "AccessToMyLaSchools";
    public const string AccessToMyMatSchools = "AccessToMyMatSchools";
    public const string AccessToAllSchools = "AccessToAllSchools";
    public const string AccessToAllLocalAuthorities = "AccessToAllLocalAuthorities";
    public const string AccessToMyDioceseSchools = "AccessToMyDioceseSchools";
    public const string AccessToGuidance = "AccessToGuidance";
    public const string AccessToEditPages = "AccessToEditPages";
    public const string AdminOnly = "AdminOnly";
    public const string NamedData = "NamedData";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Any, policy =>
            policy.RequireRole(Role.Any));

        options.AddPolicy(AccessToSearch, policy =>
            policy.RequireRole(Role.AccessToSearch));

        options.AddPolicy(AccessToMySchool, policy =>
            policy.RequireRole(Role.AccessToMySchool));

        options.AddPolicy(AccessToMySchools, policy =>
            policy.RequireRole(Role.AccessToMySchools));

        options.AddPolicy(AccessToMyLaSchools, policy =>
            policy.RequireRole(Role.AccessToMyLaSchools));

        options.AddPolicy(AccessToMyMatSchools, policy =>
            policy.RequireRole(Role.AccessToMyMatSchools));

        options.AddPolicy(AccessToMyDioceseSchools, policy =>
            policy.RequireRole(Role.AccessToMyDioceseSchools));

        options.AddPolicy(AccessToAllSchools, policy =>
            policy.RequireRole(Role.AccessToAllSchools));

        options.AddPolicy(AccessToMyLocalAuthority, policy =>
            policy.RequireRole(Role.AccessToMyLocalAuthority));

        options.AddPolicy(AccessToAllLocalAuthorities, policy =>
            policy.RequireRole(Role.AccessToAllLocalAuthorities));

        options.AddPolicy(AccessToEditPages, policy =>
            policy.RequireRole(Role.AccessToEditPages));

        options.AddPolicy(AdminOnly, policy =>
            policy.RequireRole(Role.SuperAdmin));

        options.AddPolicy(NamedData, policy =>
            policy.RequireRole(Role.NamedData));

        // Add fallback policy for all routes, requiring the user to be authenticated
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Role.Any)
            .Build();
    }
}