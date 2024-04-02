using ASP.AcceptanceTests.Drivers;
using Microsoft.Playwright;

namespace ASP.Web.AcceptanceTests.Drivers
{
    // Driver for tests to interact with a group of elements on the page using Playwright (see PlaywrightWebDriver)
    internal class PlaywrightElementsDriver : IElementsDriver
    {
        private readonly ILocator _elements;
        private readonly IPage _page;

        public PlaywrightElementsDriver(ILocator elements, IPage page)
        {
            _elements = elements;
            _page = page;
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
    }
}
