using ASP.Api.FunctionalTests.Drivers;
using ASP.Infrastructure.Api;
using ASP.Test.SpecFlow;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions
{
    [Binding]
    public sealed partial class AspApiStepDefinitions
    {
        private const string HTTP_METHOD = @"(GET|POST|DELETE)";
        private const string API_ENDPOINT = @"(/api/[^\?]+)";
        private const string QUERY_STRING = @"\?(.+)"; // to match the entire query string including spaces.
        private const string STATUS_CODE = @"(\d+)";
        private const string RESPONSE_MESSAGE = @"""(.+)""";
        private const string HTTP_HEADER = @"""([^:""]+): ([^:""]+)""";

        private readonly AspApiContext _api;
        private readonly ISpecFlowOutputHelper _output;

        public AspApiStepDefinitions(AspApiContext api, ISpecFlowOutputHelper output)
        {
            _api = api;
            _output = output;
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT}")]
        public async Task WhenISendARequest(string method, string function)
        {
            var request = new TransportLayerRequest { Method = method, Path = function };

            await _api.Run(request);
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT}{QUERY_STRING}")]
        public async Task WhenISendARequest(string method, string function, string queryString)
        {
            var request = new TransportLayerRequest { Method = method, Path = function, QueryString = "?" + queryString };

            await _api.Run(request);
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT} with content:")]
        public async Task WhenISendARequestWithContent(string method, string function, string content)
        {
            var request = new TransportLayerRequest { Method = method, Path = function, Body = content };

            await _api.Run(request);
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT}{QUERY_STRING} with content:")]
        public async Task WhenISendARequestWithContent(string method, string function, string queryString, string content)
        {
            var request = new TransportLayerRequest { Method = method, Path = function, QueryString = "?" + queryString, Body = content };

            await _api.Run(request);
        }

        [Then($@"I should get a {STATUS_CODE} response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            Assert.Equal(statusCode, _api.LastResponse.StatusCode);
        }

        [Then($@"the response should be the message {RESPONSE_MESSAGE}")]
        public void ThenTheResponseShouldBeTheMessage(string message)
        {
            Assert.Equal(message, _api.LastResponse.Body);
        }

        [Then($@"the response should include the header {HTTP_HEADER}")]
        public void ThenTheResponseShouldIncludeTheHeader(string name, string value)
        {
            Assert.Contains(name, _api.LastResponse.Headers.Keys);
            var header = _api.LastResponse.Headers[name];

            Assert.Equal(value, header);
        }

        [Then($@"the response should be an object with these exact properties:")]
        public void ThenTheResponseShouldBeAnObjectWithTheseExactProperties(string expectedContent)
        {
            Assert.Equal(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(expectedContent)), _api.LastResponse.Body);
        }

        [Then($@"the response should be an object containing these properties:")]
        public void ThenTheResponseShouldBeAnObjectContainingTheseProperties(string expectedContent)
        {
            AssertObjects.MatchProperties(expectedContent, _api.LastResponse?.Body ?? "");
        }
        
        [Then($@"the response should be an object containing these properties excluding null:")]
        public void ThenTheResponseShouldBeAnObjectContainingThesePropertiesExcludingNull(string expectedContent)
        {
            AssertObjects.MatchPropertiesExcludingNullValues(expectedContent, _api.LastResponse?.Body ?? "");
        }
        
        [Then($@"the response should be an array of objects containing these properties:")]
        public void ThenTheResponseShouldBeAnArrayOfObjectsContainingTheseProperties(string expectedContent)
        {
            AssertObjects.MatchPropertiesExcludingNullValues(expectedContent, _api.LastResponse?.Body ?? "");
        }
    }
}
