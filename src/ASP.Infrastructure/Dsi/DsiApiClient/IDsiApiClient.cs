using ASP.Infrastructure.Dsi.Models;

namespace ASP.Infrastructure.Dsi.DsiApiClient
{
    public interface IDsiApiClient
    {
        Task<UserAccess?> GetUserAccess(string serviceId, string organisationId, string userId);
    }
}
