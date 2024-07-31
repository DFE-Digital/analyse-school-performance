using ASP.Core.Results;
using ASP.Infrastructure.Dsi.Models;

namespace ASP.Infrastructure.Dsi.DsiApiClient
{
    public interface IDsiApiClient
    {
        Task<Result<UserAccess>> GetUserAccess(string serviceId, string organisationId, string userId);
    }
}
