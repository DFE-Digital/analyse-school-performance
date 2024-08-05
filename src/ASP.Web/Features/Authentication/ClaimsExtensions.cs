using ASP.Infrastructure.Dsi;
using ASP.Infrastructure.Dsi.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ASP.Web.Features.Authentication
{
    public static class ClaimExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            return principal.Claims
                .Where(c => c.Type.Contains(DsiConstants.NameIdentifier))
                .Select(c => c.Value)
                .Single();
        }

        public static Organisation? GetOrganisation(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var organisationJson = principal.Claims.Where(c => c.Type == DsiConstants.Organisation)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (organisationJson is null)
            {
                return null;
            }

            var organisation = JsonConvert.DeserializeObject<Organisation>(organisationJson)!;

            if (organisation.Id == String.Empty)
            {
                return null;
            }

            return organisation;
        }
    }
}
