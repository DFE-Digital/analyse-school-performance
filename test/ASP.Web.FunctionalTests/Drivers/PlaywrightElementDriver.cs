using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using Microsoft.Playwright;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.AcceptanceTests.Drivers
{
    // Driver for tests to interact with an element on the page using Playwright (see PlaywrightWebDriver)
    internal class PlaywrightElementDriver : IElementDriver
    {
        private ILocator _element;
        private IPage _page;
        private PlaywrightWebDriver _web;
        private ISpecFlowOutputHelper _outputHelper;

        public PlaywrightElementDriver(ILocator element, IPage page, PlaywrightWebDriver web, ISpecFlowOutputHelper outputHelper)
        {
            _element = element;
            _page = page;
            _web = web;
            _outputHelper = outputHelper;
        }

        public IElementDriver Element(string selector)
        {
            var element = _element.Locator(selector);
            return new PlaywrightElementDriver(element, _page, _web, _outputHelper);
        }

        public IElementDriver ElementByLabel(string labelText)
        {
            var element = _element.GetByLabel(labelText);
            return new PlaywrightElementDriver(element, _page, _web, _outputHelper);
        }

        public IElementsDriver Elements(string selector)
        {
            var elements = _element.Locator(selector);
            return new PlaywrightElementsDriver(elements, _page);
        }

        public async Task ShouldHaveCountAsync(int count, Func<int, string> errorIfIncorrectCount)
        {
            try
            {
                await Assertions.Expect(_element).ToHaveCountAsync(count);
            }
            catch (PlaywrightException)
            {
                var actual = await _element.CountAsync();

                AssertWithMessage.Fail(errorIfIncorrectCount(actual));
            }
        }

        public async Task ShouldNotExistAsync(string errorIfExists)
        {
            var count = await _element.CountAsync();
            if (count != 0)
            {
                var pageContent = await _web.PageContentAsync();
                _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
                AssertWithMessage.Fail(errorIfExists);
            }
        }

        public async Task<IElementDriver> ShouldExistAsync(string errorIfNotExists)
        {
            var count = await _element.CountAsync();
            if (count == 0)
            {
                var pageContent = await _web.PageContentAsync();
                _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
                AssertWithMessage.Fail(errorIfNotExists);
            }

            return this;
        }

        public async Task<string> OuterHtmlAsync()
        {
            var outerHtml = await _element.EvaluateAsync("node => node.outerHTML");

            return outerHtml.ToString() ?? "";
        }

        public async Task<string> TextContentAsync()
        {
            return await _element.TextContentAsync() ?? "";
        }

        public async Task<string> ImmediateTextContentAsync()
        {
            var text = await _element.EvaluateAsync("node => [...node.childNodes].filter(e => e.nodeType === Node.TEXT_NODE).map(e => e.textContent).join('')");
            
            return text.ToString() ?? "";
        }

        public async Task<string> InnerHtmlAsync()
        {
            return await _element.InnerHTMLAsync() ?? "";
        }

        public async Task<string> TagNameAsync()
        {
            var tagName = await _element.EvaluateAsync("node => node.tagName");

            return tagName.ToString()?.ToLower() ?? "";
        }

        public async Task<string> AttributeAsync(string attributeName)
        {
            var attributeValue = await _element.GetAttributeAsync(attributeName);

            return attributeValue ?? "";
        }

        public async Task<string> ValueAsync()
        {
            return await _element.InputValueAsync() ?? "";
        }

        public async Task<bool> IsCheckedAsync()
        {
            return await _element.IsCheckedAsync();
        }

        public async Task<bool> MatchesAsync(string selector)
        {
            var thisCount = await _element.CountAsync();
            var count = await _element.And(_page.Locator(selector)).CountAsync();

            return thisCount == count;
        }

        public async Task SetValueAsync(string value)
        {
            await _element.FillAsync(value);
        }

        public async Task SetCheckedAsync(bool isChecked)
        {
            await _element.SetCheckedAsync(isChecked);
        }

        public async Task ClickAsync()
        {
            await _element.ClickAsync();
        }
    }
}
