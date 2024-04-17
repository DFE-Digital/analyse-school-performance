using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    // Step definitions for test scenarios that interact with the ASP web application
    [Binding]
    public class AspWebStepDefinitions
    {
        private readonly IWebDriver _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(IWebDriver web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [Given(@"I navigate to ((?:/.*)+)")]
        [When(@"I navigate to ((?:/.*)+)")]
        public async Task INavigateTo(string path)
        {
            await _web.NavigateAsync(path);
        }

        [When(@"the application throws an exception")]
        public async Task TheApplicationThrowsAnException()
        {
            await _web.NavigateAsync("/error-test/throw-exception");
        }

        [Then(@"The path should match ((?:/.*)+)")]
        public void ThePathShouldMatch(string path)
        {
            Assert.Equal(_web.BaseAddress + path, _web.Path);
        }

        [When(@"I update the element ""([^""]*)"" to be (checked|unchecked)")]
        public async Task WhenIUpdateTheElementToBe(string selector, string state)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            await element.SetCheckedAsync(state == "checked");
        }

        [When(@"I update the textbox ""([^""]*)"" to have the value ""([^""]*)""")]
        public async Task WhenIUpdateTheTextBoxToHaveTheValue(string selector, string value)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            await element.SetValueAsync(value);
        }

        [When(@"I click the button ""([^""]*)""")]
        public async Task WhenIClickTheButton(string elementSelector)
        {
            var button = await _web.Element(elementSelector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{elementSelector}"".");

            await button.ClickAsync();
        }

        [Then(@"I should get a (.*) response")]
        public async Task ThenIShouldGetAResponse(int statusCode)
        {
            if (_web.Status != HttpStatusCode.OK)
            {
                var pageContent = await _web.PageContentAsync();
                _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
            }

            Assert.Equal((HttpStatusCode)statusCode, _web.Status);
        }

        [Then(@"the page title should be ""([^""]*)""")]
        public async Task ThenThePageTitleShouldBe(string expected)
        {
            var actual = await _web.PageTitleAsync();

            Assert.Equal(expected, actual);
        }

        [Then(@"the element ""([^""]*)"" should not exist")]
        public async Task ThenTheElementShouldNotExist(string selector)
        {
            await _web.Element(selector)
                .ShouldNotExistAsync(@$"Found an element with the selector ""{selector}"".");
        }

        [Then(@"the element ""([^""]*)"" should exist")]
        public async Task ThenTheElementShouldExist(string selector)
        {
            await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");
        }

        [Then(@"the element ""([^""]*)"" should have the outer HTML:")]
        public async Task ThenTheElementShouldHaveTheOuterHtml(string selector, string expectedHtml)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var outerHtml = await element.OuterHtmlAsync();
            AssertHtml.Equal(expectedHtml, outerHtml, _outputHelper.WriteLine);
        }

        [Then(@"the element ""([^""]*)"" should have the text content ""(.*)""")]
        public async Task ThenTheElementShouldHaveTheTextContent(string selector, string expectedTextContent)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var textContent = await element.TextContentAsync();
            Assert.Equal(expectedTextContent.Trim(), textContent.Trim());
        }

        [Then(@"the element ""([^""]*)"" should match the selector ""(.+)""")]
        public async Task ThenTheElementShouldMatchTheSelector(string selector, string selectorToMatch)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var matches = await element.MatchesAsync(selectorToMatch);
            AssertWithMessage.True(matches, @$"The element did not match ""{selectorToMatch}"".");
        }

        [Then(@"the element ""([^""]*)"" should have the tag name ""([^""]*)""")]
        public async Task ThenTheElementShouldHaveTheTagName(string selector, string expectedTagName)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var tagName = await element.TagNameAsync();
            Assert.Equal(expectedTagName, tagName);
        }

        [Then(@"the element ""([^""]*)"" should have the class ""([^""]*)""")]
        public async Task ThenTheElementShouldHaveTheClass(string selector, string expectedClass)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var @class = await element.AttributeAsync("class");
            Assert.Equal(expectedClass, @class);
        }

        [Then(@"the textbox ""([^""]*)"" should have the value ""([^""]*)""")]
        public async Task ThenTheTextBoxShouldHaveTheValue(string selector, string expectedValue)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var value = await element.ValueAsync();
            Assert.Equal(expectedValue.Trim(), value.Trim());
        }

        [Then(@"the element ""([^""]*)"" should be (checked|unchecked)")]
        public async Task ThenTheElementShouldBe(string selector, string state)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var isChecked = await element.IsCheckedAsync();
            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the element ""([^""]*)"" should be an internal link to ""([^""]*)""")]
        public async Task ThenTheElementShouldBeAnInternalLinkTo(string selector, string location)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var tagName = await element.TagNameAsync();
            var href = await element.AttributeAsync("href");

            Assert.Equal("a", tagName);
            Assert.Equal(location.Trim(), href.Trim());
        }

        [Then(@"the element ""([^""]*)"" should be an external link to ""([^""]*)""")]
        public async Task ThenTheElementShouldBeAnExternalLinkTo(string selector, string location)
        {
            var element = await _web.Element(selector)
                .ShouldExistAsync(@$"Could not find an element with the selector ""{selector}"".");

            var tagName = await element.TagNameAsync();
            var href = await element.AttributeAsync("href");

            Assert.Equal("a", tagName);
            Assert.Equal(location.Trim(), href.Trim());
        }

        [Then(@"the field labelled ""(.+)"" should have the value ""(.*)""")]
        public async Task ThenTheFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var field = await _web.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actualValue = await field.ValueAsync();
            Assert.Equal(expectedValue, actualValue);
        }

        [Then(@"The number of ""([^""]*)"" elements on the page should equal (.*)")]
        public async Task ThenTheNumberOfElementsOnThePageShouldEqual(string selector, int expectedCount)
        {
            var elements = _web.Elements(selector);
            var count = await elements.CountAsync();

            Assert.Equal(expectedCount, count);
        }

        [Then(@"the elements ""([^""]*)"" should have the text contents:")]
        public async Task ThenTheElementsShouldHaveTheTextContents(string selector, Table content)
        {
            int index = 0;
            var elements = _web.Elements(selector);
            var textContents = await elements.TextContentsAsync();

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), textContents[index].Trim());
                index++;
            }
        }

        [Then(@"the elements ""([^""]*)"" should have the hrefs:")]
        public async Task ThenTheElementsShouldHaveTheHrefs(string selector, Table content)
        {
            int index = 0;
            var elements = _web.Elements(selector);
            var hrefs = await elements.AttributeValuesAsync("href");

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), hrefs[index].Trim());
                index++;
            }
        }
    }
}
