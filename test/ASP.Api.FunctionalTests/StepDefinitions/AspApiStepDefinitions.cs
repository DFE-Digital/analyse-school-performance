using ASP.Api.FunctionalTests.Drivers;
using ASP.Infrastructure.Api;
using ASP.Test.SpecFlow;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text.RegularExpressions;
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
            Assert.Equal(message, _api.LastResponse.BodyString);
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
            Assert.Equal(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(expectedContent)), _api.LastResponse.BodyString);
        }

        [Then($@"the response should be an object containing these properties:")]
        public void ThenTheResponseShouldBeAnObjectContainingTheseProperties(string expectedContent)
        {
            AssertObjects.MatchProperties(expectedContent, _api.LastResponse?.BodyString ?? "");
        }

        [Then($@"the response should be an object containing these properties excluding null:")]
        public void ThenTheResponseShouldBeAnObjectContainingThesePropertiesExcludingNull(string expectedContent)
        {
            AssertObjects.MatchPropertiesExcludingNullValues(expectedContent, _api.LastResponse?.BodyString ?? "");
        }

        [Then($@"the response should be an array of objects containing these properties:")]
        public void ThenTheResponseShouldBeAnArrayOfObjectsContainingTheseProperties(string expectedContent)
        {
            AssertObjects.MatchPropertiesExcludingNullValues(expectedContent, _api.LastResponse?.BodyString ?? "");
        }

        [Then(@"the ZIP file should contain (.*) CSV files with at least (.*) rows each")]
        public async Task ThenTheZipFileShouldContainCsvFiles(int expectedFileCount, int minRowCount)
        {
            var responseBody = _api.LastResponse.BodyStream;
            Assert.NotNull(responseBody);

            using var archive = new ZipArchive(responseBody);

            var csvEntries = archive.Entries.Where(entry => entry.FullName.EndsWith(".csv")).ToList();
            Assert.Equal(expectedFileCount, csvEntries.Count);

            foreach (var csvEntry in csvEntries)
            {
                using var reader = new StreamReader(csvEntry.Open());
                var csvContent = await reader.ReadToEndAsync();

                var rows = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                Assert.True(rows.Length >= minRowCount, $"File {csvEntry.FullName} contains less than {minRowCount} rows.");
            }
        }

        [Then(@"the ZIP file name should follow the expected format")]
        public async Task ThenTheZipFileNameShouldFollowTheExpectedFormat()
        {
            var responseBody = _api.LastResponse.BodyStream;
            Assert.NotNull(responseBody);

            var contentDispositionHeader = _api.LastResponse.Headers["Content-Disposition"];
            var fileName = contentDispositionHeader
                .Split(';')
                .Select(part => part.Trim())
                .FirstOrDefault(part => part.StartsWith("filename="))?
                .Split('=')[1]
                .Trim('"');

            Assert.NotNull(fileName);

            // Expected format: "yyyyMMdd_HHmmss_download.zip"
            var regexPattern = @"^\d{8}_\d{6}_asp_download\.zip$";
            Assert.Matches(regexPattern, fileName);
        }
    }
}