using ASP.Core.Authorisation;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.FunctionalTests.Services;

public static class TestPolicy
{
    public const string DfENamedPolicy = "ASP DfE Named";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(DfENamedPolicy, policy =>
        {
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == Roles.DfeNamed));
        });
    }
}