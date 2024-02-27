using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ASP.AcceptanceTests.Drivers;
using ASP.Test.Core;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public sealed partial class AspWebStepDefinitions
    {
        private readonly AspWebContext _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(AspWebContext web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [Given(@"I navigate to ((?:/.*)+)")]
        [When(@"I navigate to ((?:/.*)+)")]
        public async Task INavigateTo(string path)
        {
            await _web.GetAsync(path);
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

        [Then(@"I should get a (.*) response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            if (_web.LastResponse.StatusCode != HttpStatusCode.OK)
            {
                _outputHelper.WriteLine(_web.LastResponse.Body.Html());
            }

            Assert.Equal(statusCode, (int)_web.LastResponse.StatusCode);
        }

        [Then(@"the element ""([^""]*)"" should have the following markup:")]
        public void ThenTheElementShouldHaveTheFollowingMarkup(string selector, string expectedMarkup)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var expected = CreateElement(expectedMarkup);

            AssertHtml.Equivalent(expected, element);
        }

        [Then(@"the element ""([^""]*)"" should have the text content ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheTextContent(string selector, string textContent)
        {
            var element = _web.LastResponse.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(textContent.Trim(), element!.TextContent.Trim());
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
            AssertWithMessage.True(element == null, @$"Found an element with the selector ""{selector}"" but it should not exist.");
        }

        private IElement CreateElement(string expectedMarkup)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument($@"<div id=""__test__"">{expectedMarkup}</div>");
            return document!.QuerySelector("#__test__ > *")!;
        }
    }
}
