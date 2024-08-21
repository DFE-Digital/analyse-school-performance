using ASP.Core.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.FunctionalTests.Services;

public static class TestPolicy
{
    public const string DfENamedPolicy = "ASP DfE Named";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(DfENamedPolicy, policy =>
        {
            policy.RequireAssertion(context => context.User.HasRole(Role.DfeNamed));
        });
    }
}