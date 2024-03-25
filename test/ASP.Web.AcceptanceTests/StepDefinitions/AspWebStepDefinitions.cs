using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.StepDefinitions
{

    [Binding]
    public partial class AspWebStepDefinitions
    {
        private readonly AspWebContext _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(AspWebContext web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [BeforeScenario]
        public void ClearDownPageContent()
        {
            _web.CookieProvider.ClearCookies();
        }

        [Given(@"I navigate to ((?:/.*)+)")]
        [When(@"I navigate to ((?:/.*)+)")]
        public async Task INavigateTo(string path)
        {
            await _web.GetAsync(path);
        }

        [Then(@"the cookie ""(.*)"" should be set to ""(.*)""")]
        public void ThenTheCookieShouldBeSetTo(string key, string value)
        {
            var cookieValue = _web.CookieProvider.GetCookie(key);

            Assert.Equal(cookieValue, value);
        }

        [When(@"the cookie ""(.*)"" has been set to ""(.*)""")]
        public void WhenTheCookieHasBeenSetTo(string key, string value)
        {
            _web.CookieProvider.SetCookie(key, value);
        }

        [Then(@"the radio ""([^""]*)"" is checked")]
        public void ThenTheRadioIsChecked(string selector)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);
            Assert.True(input.IsChecked());
        }

        [When(@"I check the radio button ""([^""]*)""")]
        public void WhenICheckTheRadioButton(string selector)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);
            var radios = _web.LastResponse.QuerySelectorAll<IHtmlInputElement>(@$"[name=""{input.Name}""]");
            foreach (var radio in radios)
            {
                radio.IsChecked = false;
            }

            input.IsChecked = true;
        }

        [Then(@"The number of ""([^""]*)"" elements on the page should equal (.*)")]
        public void ThenTheNumberOfElementsOnThePageShouldEqual(string selector, int count)
        {
            var elements = _web.LastResponse.QuerySelectorAll(selector);

            Assert.Equal(count, elements.Count());
        }


        [When(@"I update the textbox ""([^""]*)"" to have the value ""([^""]*)""")]
        public void WhenIUpdateTheTextBoxToHaveTheValue(string selector, string value)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);
            input.Value = value;
        }

        [When(@"I submit the form ""([^""]*)""")]
        public async Task WhenISubmitTheForm(string formSelector)
        {
            var element = _web.LastResponse.QuerySelector(formSelector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{formSelector}"".");
            var form = Assert.IsAssignableFrom<IHtmlFormElement>(element);

            await _web.SubmitFormAsync(form);
        }

        [When(@"I submit the form ""([^""]*)"" using the element ""([^""]*)""")]
        public async Task whenISubmitTheFormUsingTheElement(string formSelector, string elementSelector)
        {
            var formElement = _web.LastResponse.QuerySelector(formSelector);
            AssertWithMessage.NotNull(formElement, @$"Could not find an element with the selector ""{formSelector}"".");
            var form = Assert.IsAssignableFrom<IHtmlFormElement>(formElement);

            var inputElement = _web.LastResponse.QuerySelector(elementSelector);
            AssertWithMessage.NotNull(inputElement, @$"Could not find an element with the selector ""{elementSelector}"".");
            var element = Assert.IsAssignableFrom<IHtmlElement>(inputElement);

            await _web.SubmitFormAsync(form, element);
        }

        [Then(@"I should get a (.*) response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            if (_web.LastResponse.StatusCode != HttpStatusCode.OK)
            {
                _outputHelper.WriteLine(_web.LastResponse.Body.Html());
            }

            Assert.Equal(statusCode, (int)_web.LastResponse.StatusCode);
        }

        [Then(@"the page title should be ""([^""]*)""")]
        public void ThenThePageTitleShouldBe(string expected)
        {
            var actual = _web.LastResponse.Title;

            Assert.Equal(expected, actual);
        }

        [Then(@"the element ""([^""]*)"" should not exist")]
        public void ThenTheElementShouldNotExist(string selector)
        {
            AssertWithMessage.NotNull(_web.LastResponse, "No web response received. Is the test missing an action?");

            IElement? element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.Null(element, @$"Found an element with the selector ""{selector}"".");
        }

        [Then(@"the element ""([^""]*)"" should exist")]
        public void ThenTheElementShouldExist(string selector)
        {
            AssertWithMessage.NotNull(_web.LastResponse, "No web response received. Is the test missing an action?");

            IElement? element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");
        }

        [Then(@"the element ""([^""]*)"" should have the following markup:")]
        public void ThenTheElementShouldHaveTheFollowingMarkup(string selector, string expectedMarkup)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            AssertHtml.Equivalent(expectedMarkup, element!, _outputHelper.WriteLine);
        }

        [Then(@"the elements ""([^""]*)"" should have the following content")]
        public void ThenTheElementsShouldHaveTheFollowingContent(string selector, Table content)
        {
            int index = 0;
            var elements = _web.LastResponse.QuerySelectorAll(selector);

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), elements[index].TextContent.Trim());
                index++;
            }
        }

        [Then(@"the anchors ""([^""]*)"" should have the following URLs")]
        public void ThenTheAnchorsShouldHaveTheFollowingUrls(string selector, Table content)
        {
            int index = 0;
            var elements = _web.LastResponse.QuerySelectorAll(selector);

            foreach (var item in content.Rows)
            {
                var anchor = Assert.IsAssignableFrom<IHtmlAnchorElement>(elements[index]);

                Assert.Equal(item.Values.First(), anchor.PathName.Trim());
                index++;
            }
        }

        [Then(@"the element ""([^""]*)"" should have the text content ""(.*)""")]
        public void ThenTheElementShouldHaveTheTextContent(string selector, string textContent)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(textContent.Trim(), element!.TextContent.Trim());
        }

        [Then(@"the element ""([^""]*)"" should match the selector ""(.+)""")]
        public void ThenTheElementShouldMatchTheSelector(string selector, string selectorToMatch)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            AssertWithMessage.True(element!.Matches(selectorToMatch), @$"The element did not match ""{selectorToMatch}"".");
        }

        [Then(@"the element ""([^""]*)"" should have the tag name ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheTagName(string selector, string expectedTagName)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(expectedTagName, element!.TagName.ToLower());
        }

        [Then(@"the element ""([^""]*)"" should have the class ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheClass(string selector, string expectedClass)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(expectedClass, element!.ClassName);
        }

        [Then(@"the textbox ""([^""]*)"" should have the value ""([^""]*)""")]
        public void ThenTheTextBoxShouldHaveTheValue(string selector, string value)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);

            Assert.Equal(value.Trim(), input!.Value.Trim());
        }

        [Then(@"the anchor ""([^""]*)"" should be an internal link to ""([^""]*)""")]
        public void ThenTheAnchorShouldBeAnInternalLinkTo(string selector, string location)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var anchor = Assert.IsAssignableFrom<IHtmlAnchorElement>(element);

            Assert.Equal($"http://localhost{location.Trim()}", anchor.Href.Trim());
        }

        [Then(@"the anchor ""([^""]*)"" should be an external link to ""([^""]*)""")]
        public void ThenTheAnchorShouldBeAnExternalLinkTo(string selector, string location)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var anchor = Assert.IsAssignableFrom<IHtmlAnchorElement>(element);

            Assert.Equal($"{location.Trim()}", anchor.Href.Trim());
        }

        [Then(@"the field labelled ""(.+)"" should have the value ""(.*)""")]
        public void ThenTheFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var label = _web.LastResponse!.QuerySelectorAll("label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, @$"Could not find a label with the text ""{labelText}"".");

            var field = _web.LastResponse!.QuerySelector($"#{label?.Attributes["for"]?.Value}");
            AssertWithMessage.NotNull(field, @$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var value = field switch
            {
                IHtmlSelectElement select => select.Value,
                IHtmlInputElement input => input.Value,
                _ => throw new XunitException($"Could not find the value of element of type {field!.GetType().Name}.")
            };

            Assert.Equal(expectedValue, value);
        }
    }
}
