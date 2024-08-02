using ASP.Core.Authorisation;
using ASP.Web.Features.Authorisation.LocalAuthority;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation;

public static class Policy
{
    public const string LocalAuthorityAccessPolicy = "LocalAuthorityAccessPolicy";

    public const string Any = "Any";

    public const string AccessToMyLa = "AccessToMyLa";

    public const string AccessToSearch = "AccessToSearch";

    public const string AccessToMySchool = "AccessToMySchool";
    public const string AccessToMySchools = "AccessToMySchools";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Any, policy => policy.RequireAssertion(context =>
                                   context.User.HasClaim(c =>
                                      c.Type == System.Security.Claims.ClaimTypes.Role && Roles.All.Contains(c.Value))));

        options.AddPolicy(AccessToSearch, policy => policy.RequireAssertion(context =>
                                   context.User.HasClaim(c =>
                                       c.Type == System.Security.Claims.ClaimTypes.Role && Roles.AccessToSearch.Contains(c.Value))));

        options.AddPolicy(AccessToMySchool, policy => policy.RequireAssertion(context =>
                                   context.User.HasClaim(c =>
                                       c.Type == System.Security.Claims.ClaimTypes.Role && Roles.AccessToMySchool.Contains(c.Value))));

        options.AddPolicy(AccessToMySchools, policy => policy.RequireAssertion(context =>
                                   context.User.HasClaim(c =>
                                       c.Type == System.Security.Claims.ClaimTypes.Role && Roles.AccessToMySchools.Contains(c.Value))));

        options.AddPolicy(AccessToMyLa, policy => policy.RequireAssertion(context =>
                                   context.User.HasClaim(c =>
                                       c.Type == System.Security.Claims.ClaimTypes.Role && Roles.AccessToMyLa.Contains(c.Value))));

        options.AddPolicy(LocalAuthorityAccessPolicy, policy =>
            policy.Requirements.Add(new LocalAuthorityRequirement()));
    }
}