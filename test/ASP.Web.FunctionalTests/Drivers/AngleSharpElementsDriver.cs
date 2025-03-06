using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Xml.Linq;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.Web.FunctionalTests.Drivers
{
    // Driver for tests to interact with a group of elements on the page using AngleSharp (see AngleSharpWebDriver)
    public class AngleSharpElementsDriver : IElementsDriver
    {
        private readonly IElement _outerElement;
        private readonly string _selector;
        private readonly AngleSharpPage _page;
        private readonly AngleSharpWebDriver _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        private readonly IHtmlCollection<IElement> _elements;

        public AngleSharpElementsDriver(IElement outerElement, string selector, AngleSharpPage page, AngleSharpWebDriver web, ISpecFlowOutputHelper outputHelper)
        {
            _outerElement = outerElement;
            _selector = selector;
            _elements = _outerElement.QuerySelectorAll(_selector);
            _page = page;
            _web = web;
            _outputHelper = outputHelper;
        }

        public Task ShouldHaveCountAsync(int count, Func<int, string> errorIfIncorrectCount)
        {
            var actual = _elements.Count();
            Assert.Equal(count, actual, errorIfIncorrectCount(actual));

            return Task.CompletedTask;
        }

        public Task ShouldNotExistAsync(string errorIfExists)
        {
            Assert.Equal(0, _elements.Count(), errorIfExists);

            return Task.CompletedTask;
        }

        public Task<IElementsDriver> ShouldExistAsync(string errorIfNotExists)
        {
            Assert.NotEqual(0, _elements.Count(), errorIfNotExists);

            return Task.FromResult((IElementsDriver)this);
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

        public Task<IList<string>> TagNamesAsync()
        {
            return Task.FromResult((IList<string>)_elements.Select(e => e.TagName).ToList());
        }

        public Task<IList<string>> ValuesAsync()
        {
            return Task.FromResult((IList<string>)_elements.Select(e => e switch {
                IHtmlSelectElement select => select.Value,
                IHtmlInputElement input => input.Value,
                IHtmlTextAreaElement textArea => textArea.Value,
                _ => throw new XunitException($"Could not find the value of element of type {e.GetType().Name}.")
            }).ToList());
        }

        public IElementDriver ElementAt(int index)
        {
            return new AngleSharpElementDriver(_elements[index], _page, _web, _outputHelper);
        }
    }
}
