using Newtonsoft.Json;

namespace ASP.Infrastructure.Dsi.Models
{
    public class DsiCommonResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("code")]
        public string Code { get; set; } = "";
    }
}
