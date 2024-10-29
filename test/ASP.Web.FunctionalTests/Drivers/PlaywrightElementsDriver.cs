using ASP.Test.Core;
using Microsoft.Playwright;

namespace ASP.Web.FunctionalTests.Drivers
{
    // Driver for tests to interact with a group of elements on the page using Playwright (see PlaywrightWebDriver)
    internal class PlaywrightElementsDriver : IElementsDriver
    {
        private readonly ILocator _elements;
        private readonly PlaywrightPage _page;

        public PlaywrightElementsDriver(ILocator elements, PlaywrightPage page)
        {
            _elements = elements;
            _page = page;
        }

        public async Task ShouldHaveCountAsync(int count, Func<int, string> errorIfIncorrectCount)
        {
            try
            {
                await Assertions.Expect(_elements).ToHaveCountAsync(count);
            }
            catch (PlaywrightException)
            {
                var actual = await _elements.CountAsync();

                Assert.Fail(errorIfIncorrectCount(actual));
            }
        }

        public async Task ShouldNotExistAsync(string errorIfExists)
        {
            try
            {
                await Assertions.Expect(_elements).Not.ToBeVisibleAsync();
            }
            catch (PlaywrightException)
            {
                Assert.Fail(errorIfExists);
            }
        }

        public async Task<IElementsDriver> ShouldExistAsync(string errorIfNotExists)
        {
            try
            {
                await Assertions.Expect(_elements).ToBeVisibleAsync();
            }
            catch (PlaywrightException)
            {
                Assert.Fail(errorIfNotExists);
            }

            return this;
        }

        public async Task<int> CountAsync()
        {
            return await _elements.CountAsync();
        }

        public async Task<IList<string>> TextContentsAsync()
        {
            var textContents = await _elements.AllTextContentsAsync();

            return (IList<string>) textContents;
        }

        public async Task<IList<string>> AttributeValuesAsync(string attributeName)
        {
            var all = await _elements.AllAsync();
            var values = await Task.WhenAll(all.Select(e => e.GetAttributeAsync(attributeName)));

            return (values ?? []).Select(s => s ?? "").ToList();
        }

        public async Task<IList<string>> TagNamesAsync()
        {
            var all = await _elements.AllAsync();
            var values = await Task.WhenAll(all.Select(e => e.EvaluateAsync("node => node.tagName")));

            return (values ?? []).Select(s => s.ToString()?.ToLower() ?? "").ToList();
        }

        public async Task<IList<string>> ValuesAsync()
        {
            var all = await _elements.AllAsync();
            var values = await Task.WhenAll(all.Select(e => e.InputValueAsync()));

            return (values ?? []).Select(s => s ?? "").ToList();
        }
    }
}
