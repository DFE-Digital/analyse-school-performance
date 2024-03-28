using ASP.AcceptanceTests.Drivers;
using ASP.Core.Helpers;
using ASP.Test.Core;
using System.Net;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    // Step definitions for test scenarios that exercise a specific component in isolation
    [Binding]
    public partial class ComponentStepDefinitions
    {
        private readonly IWebDriver _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public ComponentStepDefinitions(IWebDriver web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [Given(@"I view the component on the page")]
        [When(@"I view the component on the page")]
        public async Task IViewTheComponentOnThePage()
        {
            await _web.NavigateAsync("/component-test/view");
        }

        [Given(@"I edit the component on the page")]
        [When(@"I edit the component on the page")]
        public async Task IEditTheComponentOnThePage()
        {
            await _web.NavigateAsync("/component-test/edit");
        }

        [When(@"I save the component")]
        public async Task WhenISaveTheComponent()
        {
            var saveButton = await _web.Element("#test-save")
                .ShouldExistAsync(@$"Could not find an element with the selector ""#test-save"".");

            await saveButton.ClickAsync();
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value ""(.*)""")]
        public async Task WhenIUpdateTheComponentFieldLabelledToHaveTheValue(string labelText, string value)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            await field.SetValueAsync(value);
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value:")]
        public async Task WhenIUpdateTheComponentFieldLabelledToHaveTheValueMultiline(string labelText, string value)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            await field.SetValueAsync(value);
        }

        [When(@"I update the component field labelled ""(.+)"" to be (checked|unchecked)")]
        public async Task WhenIUpdateTheComponentFieldLabelledToBe(string labelText, string state)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            await field.SetCheckedAsync(state == "checked");
        }

        [When(@"I click on the element ""(.+)"" within the component")]
        public async Task WhenIClickOnTheElementWithinTheComponent(string selector)
        {
            var element = await ComponentElementShouldExistAsync(selector);

            await element.ClickAsync();
        }

        [Then(@"there should be no errors")]
        public async Task ThenThereShouldBeNoErrors()
        {
            if (_web.Status != HttpStatusCode.OK)
            {
                var pageContent = await _web.PageContentAsync();
                _outputHelper.WriteLine(pageContent);
            }

            Assert.Equal(HttpStatusCode.OK, _web.Status);
        }

        [Then(@"the component should exist")]
        public async Task ThenTheComponentShouldExist()
        {
            await ComponentShouldExistAsync();
        }

        [Then(@"the component should not exist")]
        public async Task ThenTheComponentShouldNotExist()
        {
            await ComponentShouldNotExistAsync();
        }

        [Then(@"the component should have the outer HTML:")]
        public async Task ThenTheComponentShouldHaveTheOuterHtml(string expectedHtml)
        {
            var component = await ComponentShouldExistAsync();
            var html = await component.OuterHtmlAsync();

            AssertHtml.Equal(expectedHtml, html, _outputHelper.WriteLine);
        }

        [Then(@"the component should have the text content ""(.*)""")]
        public async Task ThenTheComponentShouldHaveTheTextContent(string expectedText)
        {
            var component = await ComponentShouldExistAsync();
            var textContent = await component.TextContentAsync();

            Assert.Equal(expectedText.Trim(), textContent.Trim());
        }

        [Then(@"the component should have the inner HTML ""(.*)""")]
        public async Task ThenTheComponentShouldHaveTheInnerHtml(string expectedHtml)
        {
            var component = await ComponentShouldExistAsync();
            var innerHtml = await component.InnerHtmlAsync();

            AssertHtml.Equal(expectedHtml, innerHtml, _outputHelper.WriteLine);
        }

        [Then(@"the component outer element should have the tag name ""([^""]*)""")]
        public async Task ThenTheComponentOuterElementShouldHaveTheTagName(string expectedTagName)
        {
            var component = await ComponentShouldExistAsync();
            var tagName = await component.TagNameAsync();

            Assert.Equal(expectedTagName, tagName.ToLower());
        }

        [Then(@"the component outer element should have the class ""([^""]*)""")]
        public async Task ThenTheComponentOuterElementShouldHaveTheClass(string expectedClass)
        {
            var component = await ComponentShouldExistAsync();
            var @class = await component.AttributeAsync("class");

            Assert.Equal(expectedClass, @class);
        }

        [Then(@"the element ""(.+)"" within the component should exist")]
        public async Task ThenTheElementWithinTheComponentShouldExist(string selector)
        {
            await ComponentElementShouldExistAsync(selector);
        }

        [Then(@"the element ""(.+)"" within the component should not exist")]
        public async Task ThenTheElementWithinTheComponentShouldNotExist(string selector)
        {
            await ComponentElementShouldNotExistAsync(selector);
        }

        [Then(@"the element ""(.+)"" within the component should have the tag name ""([^""]+)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheTagName(string selector, string expectedTagName)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var tagName = await element.TagNameAsync();

            Assert.Equal(expectedTagName, tagName);
        }

        [Then(@"the element ""(.+)"" within the component should have the class ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheClass(string selector, string expectedClass)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var @class = await element.AttributeAsync("class");

            Assert.Equal(expectedClass, @class);
        }

        [Then(@"the element ""(.+)"" within the component should have the text content ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheTextContent(string selector, string expectedTextContent)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var textContent = await element.TextContentAsync();

            Assert.Equal(expectedTextContent.Trim(), textContent.Trim());
        }

        [Then(@"the element ""(.+)"" within the component should have the outer HTML:")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheOuterHtml(string selector, string expectedHtml)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var html = await element.OuterHtmlAsync();

            AssertHtml.Equal(expectedHtml, html, _outputHelper.WriteLine);
        }

        [Then(@"the component field labelled ""(.+)"" should have the value ""(.*)""")]
        public async Task ThenTheComponentFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");
           
            var value = await field.ValueAsync();
            Assert.Equal(expectedValue, value);
        }

        [Then(@"the component field labelled ""(.+)"" should be (checked|unchecked)")]
        public async Task ThenTheComponentFieldLabelledShouldBeUnchecked(string labelText, string state)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");
            
            var isChecked = await field.IsCheckedAsync();
            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the component field labelled ""(.+)"" should match the JSON string ""(.*)""")]
        public async Task ThenTheComponentFieldLabelledShouldMatchTheJSONString(string labelText, string expectedValue)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actualValue = await field.ValueAsync();

            var normalizedExpected = JsonHelper.Serialize(JsonHelper.Deserialize<object>(expectedValue));
            var normalizedActual = JsonHelper.Serialize(JsonHelper.Deserialize<object>(actualValue));
            Assert.Equal(normalizedExpected, normalizedActual);
        }

        [Then(@"the input element of the component field labelled ""(.+)"" should match the selector ""(.+)""")]
        public async Task ThenTheInputElementOfTheComponentFieldLabelledShouldMatchTheSelector(string labelText, string selector)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync(@$"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var matches = await field.MatchesAsync(selector);

            AssertWithMessage.True(matches, @$"The associated input element for the label ""{labelText}"" did not match ""{selector}"".");
        }

        private async Task<IElementDriver> ComponentShouldExistAsync()
        {
            await ThenThereShouldBeNoErrors();

            var testWrapper = await _web.Element("#test")
                .ShouldExistAsync(@$"Could not find an element with the selector ""#test"".");
            
            var component = await testWrapper.Element(":scope > div > *")
                .ShouldExistAsync(@$"Could not find the component's outer element on the page.");

            return component;
        }

        private async Task ComponentShouldNotExistAsync()
        {
            await ThenThereShouldBeNoErrors();

            var testWrapper = await _web.Element("#test")
                .ShouldExistAsync(@$"Could not find an element with the selector ""#test"".");

            AssertWithMessage.NotNull(testWrapper, @$"Could not find an element with the selector ""#test"".");

            await testWrapper.Element(":scope > div > *")
                .ShouldNotExistAsync(@$"Found the component's outer element on the page.");
        }

        private async Task<IElementDriver> ComponentElementShouldExistAsync(string selector)
        {
            var component = await ComponentShouldExistAsync();

            var element = await component.Element($":scope {selector}")
                .ShouldExistAsync(@$"Could not find an element within the component with the selector ""{selector}"".");

            return element;
        }

        private async Task ComponentElementShouldNotExistAsync(string selector)
        {
            var component = await ComponentShouldExistAsync();

            await component.Element($":scope {selector}")
                .ShouldNotExistAsync(@$"Found an element within the component with the selector ""{selector}"".");
        }
    }
}
