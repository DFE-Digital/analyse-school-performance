using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public class FileDownloadStepDefinitions : Test.Reqnroll.FileDownloadStepDefinitions
    {
        private readonly IWebDriver _web;

        public FileDownloadStepDefinitions(IWebDriver web, IReqnrollOutputHelper output)
            : base(output)
        {
            _web = web;
        }

        protected override Dictionary<string, string> Headers => _web.Headers;

        protected override Task<Stream> StreamAsync()
        {
            return Task.FromResult(_web.CurrentDownload.Stream);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            base.DisposeArchive();
        }

        [When(@"I click the download link ""(.+)""")]
        public async Task IClickTheDownloadLink(string selector)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.StartDownloadAsync();
        }
    }
}