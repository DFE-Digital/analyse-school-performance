using ASP.Infrastructure.Dsi.DsiApiClientProvider;
using ASP.Infrastructure.Dsi.Models;
using Newtonsoft.Json;
using System.Net;

namespace ASP.Infrastructure.Dsi.DsiApiClient
{
    //Http client used to access the DSI public API
    //see https://github.com/DFE-Digital/login.dfe.public-api
    //Currently, only the GetUserAccess is implemented - using for getting user Roles
    public class DsiApiClient : IDsiApiClient
    {
        private readonly HttpClient _dsiApiClientProvider;

        public DsiApiClient(IDsiApiClientProvider dsiApiClientProvider)
        {
            ArgumentNullException.ThrowIfNull(dsiApiClientProvider);

            _dsiApiClientProvider = dsiApiClientProvider.CreateHttpClient();
        }

        //DSI endpoint for returning user Roles
        public async Task<UserAccess?> GetUserAccess(string serviceId, string organisationId, string userId)
        {
            var endpoint = $"services/{serviceId}/organisations/{organisationId}/users/{userId}";

            var response = await _dsiApiClientProvider.GetAsync(endpoint);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // User account is not enrolled into service and has no roles.
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var userAccessToService = JsonConvert.DeserializeObject<UserAccess>(responseContent);

            return userAccessToService;
        }
    }
}
