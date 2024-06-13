using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ASP.Test.Core;
using System.Xml.Linq;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.Drivers
{
    // Driver for tests to interact with a group of elements on the page using AngleSharp (see AngleSharpWebDriver)
    public class AngleSharpElementsDriver : IElementsDriver
    {
        private IElement _outerElement;
        private string _selector;

        private readonly IHtmlCollection<IElement> _elements;
        private readonly AngleSharpWebDriver _web;

        public AngleSharpElementsDriver(IElement outerElement, string selector, AngleSharpWebDriver web)
        {
            _outerElement = outerElement;
            _selector = selector;
            _web = web;
            _elements = _outerElement.QuerySelectorAll(_selector);
        }

        public Task ShouldHaveCountAsync(int count, Func<int, string> errorIfIncorrectCount)
        {
            var actual = _elements.Count();
            AssertWithMessage.Equal(count, actual, errorIfIncorrectCount(actual));

            return Task.CompletedTask;
        }

        public Task ShouldNotExistAsync(string errorIfExists)
        {
            AssertWithMessage.Equal(0, _elements.Count(), errorIfExists);

            return Task.CompletedTask;
        }

        public Task<IElementsDriver> ShouldExistAsync(string errorIfNotExists)
        {
            AssertWithMessage.NotEqual(0, _elements.Count(), errorIfNotExists);

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
    }
}
