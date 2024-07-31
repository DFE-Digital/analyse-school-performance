using System.Security.Claims;

namespace ASP.Core.Helpers;

public static class ClaimsHelper
{
    public static string? GetFirstNonEmptyRoleClaim(ClaimsPrincipal user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
    }
}