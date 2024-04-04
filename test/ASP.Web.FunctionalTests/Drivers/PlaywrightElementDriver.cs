using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using Microsoft.Playwright;

namespace ASP.Web.AcceptanceTests.Drivers
{
    // Driver for tests to interact with an element on the page using Playwright (see PlaywrightWebDriver)
    internal class PlaywrightElementDriver : IElementDriver
    {
        private ILocator _element;
        private IPage _page;

        public PlaywrightElementDriver(ILocator element, IPage page)
        {
            _element = element;
            _page = page;
        }

        public IElementDriver Element(string selector)
        {
            var element = _element.Locator(selector);
            return new PlaywrightElementDriver(element, _page);
        }

        public IElementDriver ElementByLabel(string labelText)
        {
            var element = _element.GetByLabel(labelText);
            return new PlaywrightElementDriver(element, _page);
        }

        public IElementsDriver Elements(string selector)
        {
            var elements = _element.Locator(selector);
            return new PlaywrightElementsDriver(elements, _page);
        }

        public async Task ShouldHaveCountAsync(int count, string errorIfIncorrectCount)
        {
            try
            {
                await Assertions.Expect(_element).ToHaveCountAsync(count);
            }
            catch (PlaywrightException)
            {
                AssertWithMessage.Fail(errorIfIncorrectCount);
            }
        }

        public async Task ShouldNotExistAsync(string errorIfExists)
        {
            try
            {
                await Assertions.Expect(_element).Not.ToBeVisibleAsync();
            }
            catch (PlaywrightException)
            {
                AssertWithMessage.Fail(errorIfExists);
            }
        }

        public async Task<IElementDriver> ShouldExistAsync(string errorIfNotExists)
        {
            try
            {
                await Assertions.Expect(_element).ToBeVisibleAsync();
            }
            catch (PlaywrightException)
            {
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
