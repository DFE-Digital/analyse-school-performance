using ASP.Api.FunctionalTests.Drivers;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions
{
    [Binding]
    public class FileDownloadStepDefinitions : Test.SpecFlow.FileDownloadStepDefinitions
    {
        private readonly AspApiContext _api;

        public FileDownloadStepDefinitions(AspApiContext api, ISpecFlowOutputHelper output)
            : base(output)
        {
            _api = api;
        }

        protected override Dictionary<string, string> Headers =>
            _api.LastRequest.Headers
                .Select(JoinHeaderValues)
                .Concat(_api.LastResponse.Content?.Headers.Select(JoinHeaderValues) ?? [])
                .ToDictionary(h => h.Key, h => h.Value);

        protected override Task<Stream> StreamAsync()
        {
            Assert.NotNull(_api.LastResponse.Content, "No response content found - are you missing a file download step?");
            return _api.LastResponse.Content!.ReadAsStreamAsync();
        }

        private KeyValuePair<string, string> JoinHeaderValues(KeyValuePair<string, IEnumerable<string>> kvp) =>
            new KeyValuePair<string, string>(kvp.Key, string.Join(",", kvp.Value));

        [AfterScenario]
        public void AfterScenario()
        {
            base.DisposeArchive();
        }
    }
}