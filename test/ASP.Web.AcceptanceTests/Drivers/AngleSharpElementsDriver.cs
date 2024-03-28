using AngleSharp.Dom;

namespace ASP.AcceptanceTests.Drivers
{
    // Driver for tests to interact with a group of elements on the page using AngleSharp (see AngleSharpWebDriver)
    public class AngleSharpElementsDriver : IElementsDriver
    {
        private readonly IHtmlCollection<IElement> _elements;
        private readonly AngleSharpWebDriver _web;

        public AngleSharpElementsDriver(IHtmlCollection<IElement> elements, AngleSharpWebDriver web)
        {
            _elements = elements;
            _web = web;
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(_elements.Count());
        }

        public Task<IList<string>> TextContentsAsync()
        {
            return Task.FromResult((IList<string>) _elements.Select(e => e.TextContent).ToList());
        }

        public Task<IList<string>> AttributeValuesAsync(string attributeName)
        {
            return Task.FromResult((IList<string>)_elements.Select(e => e.GetAttribute(attributeName) ?? "").ToList());
        }
    }
}
