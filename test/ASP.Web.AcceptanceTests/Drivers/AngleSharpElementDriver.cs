using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ASP.Test.Core;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.Drivers
{
    // Driver for tests to interact with an element on the page using AngleSharp (see AngleSharpWebDriver)
    public class AngleSharpElementDriver : IElementDriver
    {
        private IElement? _element;
        private AngleSharpWebDriver _web;

        public AngleSharpElementDriver(IElement? element, AngleSharpWebDriver web)
        {
            _element = element;
            _web = web;
        }

        private IElement El
        {
            get
            {
                AssertWithMessage.NotNull(_element, "Element does not exist on the page");

                return _element;
            }
        }

        public IElementDriver Element(string selector)
        {
            var element = El.QuerySelector(selector);

            return new AngleSharpElementDriver(element, _web);
        }

        public IElementDriver ElementByLabel(string labelText)
        {
            var label = El.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = El.QuerySelector($":scope #{label?.Attributes["for"]?.Value}");

            return new AngleSharpElementDriver(field, _web);
        }

        public Task ShouldNotExistAsync(string errorIfExists)
        {
            AssertWithMessage.Null(_element, errorIfExists);

            return Task.CompletedTask;
        }

        public Task<IElementDriver> ShouldExistAsync(string errorIfNotExists)
        {
            AssertWithMessage.NotNull(_element, errorIfNotExists);

            return Task.FromResult((IElementDriver) this);
        }

        public Task<string> TextContentAsync()
        {
            return Task.FromResult(El.TextContent);
        }

        public Task<string> InnerHtmlAsync()
        {
            return Task.FromResult(El.InnerHtml);
        }

        public Task<string> TagNameAsync()
        {
            return Task.FromResult(El.TagName.ToLower());
        }

        public Task<string> OuterHtmlAsync()
        {
            return Task.FromResult(Html.Minify(El));
        }

        public Task<string> ValueAsync()
        {
            var value = El switch {
                IHtmlSelectElement select => select.Value,
                IHtmlInputElement input => input.Value,
                IHtmlTextAreaElement textArea => textArea.Value,
                _ => throw new XunitException($"Could not find the value of element of type {El.GetType().Name}.")
            };

            return Task.FromResult(value ?? "");
        }

        public Task<bool> IsCheckedAsync()
        {
            var isChecked = El switch {
                IHtmlInputElement input => input.IsChecked,
                _ => throw new XunitException($"Could not find the checked state of element of type {El.GetType().Name}.")
            };

            return Task.FromResult(isChecked);
        }

        public Task<bool> MatchesAsync(string selector)
        {
            return Task.FromResult(El.Matches(selector));
        }

        public Task SetValueAsync(string value)
        {
            var _ = El switch {
                IHtmlSelectElement select => select.Value = value,
                IHtmlInputElement input => input.Value = value,
                IHtmlTextAreaElement textarea => textarea.Value = value,
                _ => throw new XunitException($"Could not set the value of element of type {El.GetType().Name}.")
            };

            return Task.CompletedTask;
        }

        public Task SetCheckedAsync(bool isChecked)
        {
            if(isChecked)
            {
                var input = Assert.IsAssignableFrom<IHtmlInputElement>(El);
                var radios = _web.LastResponse.QuerySelectorAll<IHtmlInputElement>(@$"[name=""{input.Name}""]");

                foreach (var radio in radios)
                {
                    radio.IsChecked = false;
                }
            }

            var _ = El switch {
                IHtmlInputElement input => input.IsChecked = isChecked,
                _ => throw new XunitException($"Could not set the checked state of element of type {El.GetType().Name}.")
            };

            return Task.CompletedTask;
        }

        public Task<string> AttributeAsync(string attributeName)
        {
            var attribute = El.Attributes[attributeName];

            return Task.FromResult(attribute?.Value ?? "");
        }

        public async Task ClickAsync()
        {
            // When Javascript is disabled, any element that needs to perform a function when clicked (other than being a pure link
            // to another page) must be a submit button within a form that submits to a controller action.
            var button = AssertWithMessage.IsAssignableFrom<IHtmlButtonElement>(El, "Clicked element was not a <button> element");
            AssertWithMessage.Equal("submit", button.Type, @"Clicked element was not a submit button - expecting <button type=""submit""");

            var form = button.Ancestors<IHtmlFormElement>().FirstOrDefault();

            AssertWithMessage.NotNull(form, "Clicked element was not contained within a <form> elemnent");

            await _web.SubmitFormAsync(form, button);
        }
    }
}
