using ASP.Api.AcceptanceTests.Drivers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public sealed partial class AspApiStepDefinitions
    {
        private const string HTTP_METHOD = @"(GET|POST|DELETE)";
        private const string API_ENDPOINT = @"/([^\?]+)";
        private const string QUERY_STRING = @"\?([^ ]*)";
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
            var request = CreateRequest(method);

            await _api.Run(function, request);
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT}{QUERY_STRING}")]
        public async Task WhenISendARequest(string method, string function, string queryString)
        {
            var request = CreateRequest(method, queryString);

            await _api.Run(function, request);
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT} with content:")]
        public async Task WhenISendARequestWithContent(string method, string function, string content)
        {
            var request = CreateRequest(method);

            using (new RequestBodyWriter(request, content))
            {
                await _api.Run(function, request);
            }
        }

        [When($@"I send a {HTTP_METHOD} request to {API_ENDPOINT}{QUERY_STRING} with content:")]
        public async Task WhenISendARequestWithContent(string method, string function, string queryString, string content)
        {
            var request = CreateRequest(method, queryString);

            using (new RequestBodyWriter(request, content))
            {
                await _api.Run(function, request);
            }
        }

        [Then($@"I should get a {STATUS_CODE} response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            Assert.Equal(statusCode, _api.LastResponse.StatusCode);
        }

        [Then($@"the response should be the message {RESPONSE_MESSAGE}")]
        public void ThenTheResponseShouldBeTheMessage(string message)
        {
            Assert.Equal(message, _api.LastResponse.Value);
        }

        [Then($@"the response should include the header {HTTP_HEADER}")]
        public void ThenTheResponseShouldIncludeTheHeader(string name, string value)
        {
            Assert.Contains(name, _api.LastRequest.HttpContext.Response.Headers.Keys);
            var header = _api.LastRequest.HttpContext.Response.Headers[name];

            Assert.Equal(value, header);
        }

        [Then($@"the response should be an object with these exact properties:")]
        public void ThenTheResponseShouldBeAnObjectWithTheseExactProperties(string expectedContent)
        {
            Assert.Equal(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(expectedContent)), JsonConvert.SerializeObject(_api.LastResponse.Value));
        }

        [Then($@"the response should be an object containing these properties:")]
        public void ThenTheResponseShouldBeAnObjectContainingTheseProperties(string expectedContent)
        {
            var expectedProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(expectedContent);
            var actualProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(_api.LastResponse.Value));
            foreach(var property in expectedProperties)
            {
                Assert.Contains(property.Key, actualProperties.Keys);
                Assert.Equal(property.Value, actualProperties[property.Key]);
            }
        }
        
        [Then($@"the response should be an object containing these properties excluding null:")]
        public void ThenTheResponseShouldBeAnObjectContainingThesePropertiesExcludingNull(string expectedContent)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var expectedProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(expectedContent);
            var actualProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(_api.LastResponse.Value, settings));
            foreach(var property in expectedProperties)
            {
                Assert.Contains(property.Key, actualProperties.Keys);
                Assert.Equal(property.Value, actualProperties[property.Key]);
            }
        }

        [Then($@"the response should be an array of objects containing these properties:")]
        public void ThenTheResponseShouldBeAnArrayOfObjectsContainingTheseProperties(string expectedContent)
        {
            var expectedPropertiesList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(expectedContent);
            var actualPropertiesList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonConvert.SerializeObject(_api.LastResponse.Value));

            for (int i = 0; i < expectedPropertiesList.Count; i++)
            {
                var expectedProperties = expectedPropertiesList[i];
                var actualProperties = actualPropertiesList[i];

                foreach (var property in expectedProperties)
                {
                    Assert.Contains(property.Key, actualProperties.Keys);
                    Assert.Equal(property.Value, actualProperties[property.Key]);
                }
            }
        }
        private HttpRequest CreateRequest(string method, string? queryString = null)
        {
            var request = new DefaultHttpContext().Request;
            request.Method = method;
            
            if (queryString != null)
            {
                request.QueryString = new QueryString("?" + queryString);
            }

            return request;
        }

        private class RequestBodyWriter : IDisposable
        {
            private readonly Stream _stream;
            private readonly StreamWriter _writer;

            public RequestBodyWriter(HttpRequest request, string body)
            {
                _stream = new MemoryStream();
                _writer = new StreamWriter(_stream);
                _writer.Write(body);
                _writer.Flush();
                _stream.Position = 0;
                request.Body = _stream;
            }

            public void Dispose()
            {
                _writer.Dispose();
                _stream.Dispose();
            }
        }
    }
}
