using ASP.Api.FunctionalTests.Drivers;
using Newtonsoft.Json;

namespace ASP.Api.FunctionalTests.StepDefinitions
{
    [Binding]
    public sealed partial class AspApiStepDefinitions
    {
        private const string HTTP_METHOD = @"(GET|POST|PUT|DELETE)";
        private const string STATUS_CODE = @"(\d+)";
        private const string RESPONSE_MESSAGE = @"""(.+)""";
        private const string HTTP_HEADER = @"""([^:""]+): ([^:""]+)""";

        private readonly AspApiContext _api;
        private readonly IReqnrollOutputHelper _output;

        public AspApiStepDefinitions(AspApiContext api, IReqnrollOutputHelper output)
        {
            _api = api;
            _output = output;
        }

        [When($@"I send a {HTTP_METHOD} request to (/api/[^\s]+)")]
        public async Task WhenISendARequest(string method, string path)
        {
            var request = new HttpRequestMessage { 
                Method = HttpMethod.Parse(method), 
                RequestUri = new Uri($"https://localhost{path}") 
            };

            await _api.Run(request);
        }

        [When($@"I send a {HTTP_METHOD} request to (/api/[^\s]+) with content:")]
        public async Task WhenISendARequestWithContent(string method, string path, string content)
        {
            var request = new HttpRequestMessage { 
                Method = HttpMethod.Parse(method), 
                RequestUri = new Uri($"https://localhost{path}"),
                Content = new StringContent(content)
            };

            await _api.Run(request);
        }

        [Then($@"I should get a {STATUS_CODE} response")]
        public async Task ThenIShouldGetAResponse(int statusCode)
        {
            if (_api.LastResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();
                _output.WriteLine("Full content:");
                _output.WriteLine(responseBody);
            }

            Assert.Equal(statusCode, (int)_api.LastResponse.StatusCode);
        }

        [Then($@"the response should be the message {RESPONSE_MESSAGE}")]
        public async Task ThenTheResponseShouldBeTheMessage(string message)
        {
            var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();

            Assert.Equal(message, responseBody);
        }

        [Then($@"the response should include the header {HTTP_HEADER}")]
        public void ThenTheResponseShouldIncludeTheHeader(string name, string value)
        {
            var headerDict = _api.LastResponse.Headers.Concat(_api.LastResponse.Content.Headers).ToDictionary(h => h.Key, h => h.Value);
            Assert.Contains(name, headerDict);
            var header = headerDict[name];

            Assert.Equal(value, string.Join(",", header));
        }

        [Then($@"the response should be an object with these exact properties:")]
        public async Task ThenTheResponseShouldBeAnObjectWithTheseExactProperties(string expectedContent)
        {
            var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();

            Assert.Equal(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(expectedContent)), responseBody);
        }

        [Then($@"the response should be an object containing these properties:")]
        public async Task ThenTheResponseShouldBeAnObjectContainingTheseProperties(string expectedContent)
        {
            var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();

            Assert.ObjectMatchesProperties(expectedContent, responseBody);
        }

        [Then(@"the response should be an object containing these properties \(ignoring null values\):")]
        public async Task ThenTheResponseShouldBeAnObjectContainingThesePropertiesExcludingNull(string expectedContent)
        {
            var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();

            Assert.ObjectMatchesPropertiesExcludingNullValues(expectedContent, responseBody);
        }

        [Then($@"the response should be an array of objects containing these properties:")]
        public async Task ThenTheResponseShouldBeAnArrayOfObjectsContainingTheseProperties(string expectedContent)
        {
            var responseBody = await _api.LastResponse.Content.ReadAsStringAsync();

            Assert.ObjectMatchesPropertiesExcludingNullValues(expectedContent, responseBody);
        }
    }
}