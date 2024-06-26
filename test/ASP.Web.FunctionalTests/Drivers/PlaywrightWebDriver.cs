using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using Microsoft.Playwright;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.AcceptanceTests.Drivers
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

            _page!.Response += (object? sender, IResponse e) => this.ExpectedStatusCode = 200;
        }

        // Clear down the static resources after the test run
        // (not strictly necessary but just being tidy)
        [AfterTestRun]
        public static async Task DisposeResources()
        {
            if (_browser != null)
            {
                await _browser.DisposeAsync();
            }

            if (_playwright != null)
            {
                _playwright!.Dispose();
            }
        }

        [NotNull]
        private IResponse LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, @"No web response received. Is the test missing a ""navigate"" step?");
                }

                return _lastResponse!;
            }
        }

        [NotNull]
        private IPage Page
        {
            get
            {
                return _page!;
            }
        }

        public async Task NavigateAsync(string path)
        {
            _lastResponse = await _page!.GotoAsync($"{_web.ServerAddress.TrimEnd('/')}{path}");
            ExpectedStatusCode = 200;
        }

        public HttpStatusCode Status => (HttpStatusCode)LastResponse.Status;

        public string Path => LastResponse.Url;

        public string BaseAddress => _web.ServerAddress.TrimEnd('/') ?? string.Empty;

        public int ExpectedStatusCode { get; set; }

        public async Task<string> PageContentAsync()
        {
            return await LastResponse.TextAsync();
        }

        public async Task<string> PageTitleAsync()
        {
            return await Page.TitleAsync();
        }

        public async Task<IElementDriver> Element(string selector)
        {
            await ExpectStatusCode();

            var element = Page.Locator(selector);
            return new PlaywrightElementDriver(element, Page, this, _outputHelper);
        }

        public async Task<IElementDriver> ElementByLabel(string labelText)
        {
            await ExpectStatusCode();

            var element = Page.GetByLabel(labelText);
            return new PlaywrightElementDriver(element, Page, this, _outputHelper);
        }

        public async Task<IElementsDriver> Elements(string selector)
        {
            await ExpectStatusCode();

            var elements = Page.Locator(selector);
            return new PlaywrightElementsDriver(elements, Page);
        }

        private async Task ExpectStatusCode()
        {
            if ((int)Status != 200)
            {
                var pageContent = await PageContentAsync();
                _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
            }

            AssertWithMessage.Equal(ExpectedStatusCode, (int)Status, $"Expected response status to be {ExpectedStatusCode} but was {(int)Status}.");
        }
    }
}
