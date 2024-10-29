using System.Net;
using ASP.Test.Core;
using Microsoft.Playwright;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.Drivers
{
    // Web driver that uses Playwright to execute Javascript in a real web browser. Javascript support in AngleSharp
    // is very limited so in order to make sure any components that use Javascript are properly tested we need to use
    // a browser automation tool
    [Binding]
    public class PlaywrightWebDriver : IWebDriver
    {
        private static IPage? _page;
        private static IPlaywright? _playwright;
        private static IBrowser? _browser;
        private readonly AspWebContext _web;
        private readonly ISpecFlowOutputHelper _outputHelper;
        private PlaywrightDownload? _lastDownload;
        private PlaywrightPage? _lastPage;
        private IResponse? _lastResponse;

        public PlaywrightWebDriver(AspWebContext web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;

            // The first instance will take longer to initialize as we spin up the
            // web browser, but this is static so future instances will have access to it
            // Important: this is not thread-safe, but this does not matter as 
            // parallelization of tests is explicitly disabled, if this changes this
            // code could cause intermittent issues
            if (_playwright == null)
            {
                Task.Run(async () =>
                {
                    _playwright = await Playwright.CreateAsync();
                }).Wait();
            }

            if (_browser == null)
            {
                Task.Run(async () =>
                {
                    _browser = await _playwright!.Chromium.LaunchAsync();
                }).Wait();
            }

            if (_page == null)
            {
                Task.Run(async () =>
                {
                    _page = await _browser!.NewPageAsync();
                }).Wait();
            }

            _page!.Response += PlaywrightWebDriver_Response;
        }

        private void PlaywrightWebDriver_Response(object? sender, IResponse e)
        {
            if (e.Request.ResourceType == "document")
            {
                ExpectedStatusCode = 200;
                _lastResponse = e;
            }
        }

        public IHtmlPage CurrentPage
        {
            get
            {
                Assert.NotNull(_lastPage, @"No web response received. Is the test missing a ""navigate"" step?");

                return _lastPage!;
            }
        }

        public IDownload CurrentDownload
        {
            get
            {
                Assert.NotNull(_lastDownload, @"No web response received. Is the test missing a ""navigate"" step?");

                return _lastDownload!;
            }
        }

        public async Task NavigateAsync(string path)
        {
            var response = await _page!.GotoAsync($"{_web.ServerAddress.TrimEnd('/')}{path}");
            Assert.NotNull(response, @"Response from GotoAsync() was null");

            _lastPage = new PlaywrightPage(_page, this, response, _outputHelper);
            if(_lastDownload != null)
            {
                _lastDownload.Dispose();
                _lastDownload = null;
            }
        }

        public async Task CaptureDownloadAsync(Func<Task> action)
        {
            var waitForDownloadTask = _page!.WaitForDownloadAsync();
            await action();
            
            var download = await waitForDownloadTask;

            var stream = await download.CreateReadStreamAsync();
            _lastDownload = new PlaywrightDownload(stream, _outputHelper);
            if (_lastPage != null)
            {
                _lastPage.Dispose();
                _lastPage = null;
            }
        }

        public HttpStatusCode StatusCode => 
            (HttpStatusCode)(_lastResponse?.Status ?? 0);

        public Dictionary<string, string> Headers => 
            _lastResponse?.Headers ?? new();

        public string BaseAddress => 
            _web.ServerAddress.TrimEnd('/') ?? string.Empty;

        public int ExpectedStatusCode { get; set; }

        public async Task ExpectStatusCode()
        {
            if ((int)StatusCode != 200)
            {
                if (_lastPage != null)
                {
                    var pageContent = await _lastPage.PageContentAsync();
                    if (pageContent.StartsWith("<!DOCTYPE html>"))
                    {
                        var errorMessageElement = _lastPage.Page.Locator(@"[data-testid=""error-display-message""]");
                        var errorMessage = (await errorMessageElement.TextContentAsync())?
                            .Trim()
                            .Replace("\\u0022", "\"")
                            .Replace("\\r", "\r")
                            .Replace("\\n", "\n")
                            ?? "(none)";

                        _outputHelper.WriteLine($"Error message: {errorMessage}");

                        var stackTraceElement = _lastPage.Page.Locator(@"[data-testid=""error-stack-trace""]");
                        var stackTrace = (await stackTraceElement.TextContentAsync())?
                            .Trim()
                            .Replace("\\u0022", "\"")
                            .Replace("\\r", "\r")
                            .Replace("\\n", "\n")
                            ?? "(none)";

                        _outputHelper.WriteLine($"Stack trace: {stackTrace}");
                    }
                    else
                    {
                        _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
                    }
                }

                Assert.Equal(ExpectedStatusCode, (int)StatusCode, $"Expected response status to be {ExpectedStatusCode} but was {(int)StatusCode}.");
            }
        }

        public void Dispose()
        {
            _page!.Response -= PlaywrightWebDriver_Response;
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
