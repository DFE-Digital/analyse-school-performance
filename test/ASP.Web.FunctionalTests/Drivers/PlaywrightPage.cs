using ASP.Test.Core;
using Microsoft.Playwright;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.Drivers
{
    public class PlaywrightPage : IHtmlPage
    {
        private readonly IPage _page;
        private readonly PlaywrightWebDriver _web;
        private ISpecFlowOutputHelper _outputHelper;
        private IResponse _lastResponse;

        public PlaywrightPage(IPage page, PlaywrightWebDriver web, IResponse response, ISpecFlowOutputHelper outputHelper)
        {
            _page = page;
            _web = web;
            _outputHelper = outputHelper;
            _lastResponse = response;
            _page.Response += Page_Response;
        }

        private void Page_Response(object? sender, IResponse e)
        {
            if (e.Request.ResourceType == "document")
            {
                _lastResponse = e;
            }
        }

        public IPage Page => _page;
        public string Path => _page.Url;

        public async Task<string> TitleAsync() => await Page.TitleAsync();
        public async Task<string> PageContentAsync() => await _lastResponse.TextAsync();

        public async Task<IElementDriver> ElementAsync(string selector)
        {
            await _web.ExpectStatusCode();

            var element = Page.Locator(selector);
            return new PlaywrightElementDriver(element, this, _web, _outputHelper);
        }

        public async Task<IElementDriver> ElementByLabelAsync(string labelText)
        {
            await _web.ExpectStatusCode();

            var element = Page.GetByLabel(labelText);
            return new PlaywrightElementDriver(element, this, _web, _outputHelper);
        }

        public async Task<IElementsDriver> ElementsAsync(string selector)
        {
            await _web.ExpectStatusCode();

            var elements = Page.Locator(selector);
            return new PlaywrightElementsDriver(elements, this, _web, _outputHelper);
        }

        public async Task WaitForSelectorAsync(string selector, string errorIfNotExists)
        {
            try
            {
                await Page.WaitForSelectorAsync(selector);
            }
            catch (TimeoutException ex)
            {
                _outputHelper.WriteLine($"TimeoutException occurred while waiting for selector: {selector}");
                _outputHelper.WriteLine($"Error message: {ex.Message}");
                Assert.Fail(errorIfNotExists);
            }
            catch (Exception ex)
            {
                _outputHelper.WriteLine($"Unexpected exception occurred while waiting for selector: {selector}");
                _outputHelper.WriteLine($"Exception type: {ex.GetType().Name}");
                _outputHelper.WriteLine($"Error message: {ex.Message}");
                _outputHelper.WriteLine($"Stack trace: {ex.StackTrace}");
                Assert.Fail(errorIfNotExists);
            }
        }

        public void Dispose()
        {
            _page.Response -= Page_Response;
        }
    }
}
