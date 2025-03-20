using ASP.Core.Text;
using ASP.Test.Web.Areas.ComponentTest;
using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    // Step definitions for test scenarios that exercise a specific component in isolation
    [Binding]
    public partial class ComponentStepDefinitions
    {
        private readonly IWebDriver _web;
        private readonly IReqnrollOutputHelper _outputHelper;

        public ComponentStepDefinitions(IWebDriver web, IReqnrollOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [Given(@"I view the component on the page")]
        [When(@"I view the component on the page")]
        public async Task IViewTheComponentOnThePage()
        {
            await _web.NavigateAsync($"/component-test/view?revision={ComponentTestController.TEST_COMPONENT_TEMPLATE_ID}");
        }

        [Given(@"I edit the component on the page")]
        [When(@"I edit the component on the page")]
        public async Task IEditTheComponentOnThePage()
        {
            await _web.NavigateAsync($"/component-test/edit?revision={ComponentTestController.TEST_COMPONENT_TEMPLATE_ID}");
        }

        [When(@"I save the component")]
        public async Task WhenISaveTheComponent()
        {
            var saveButton = await _web.CurrentPage.ElementAsync("#test-save");
            await saveButton.ShouldExistAsync($@"Could not find an element with the selector ""#test-save"".");

            await saveButton.ClickAsync();
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value ""(.*)""")]
        public async Task WhenIUpdateTheComponentFieldLabelledToHaveTheValue(string labelText, string value)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            await field.SetValueAsync(value);
        }

        [When(@"I update the component field labelled ""(.+)"" to have the value:")]
        public async Task WhenIUpdateTheComponentFieldLabelledToHaveTheValueMultiline(string labelText, string value)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            await field.SetValueAsync(value);
        }

        [When(@"I update the component field labelled ""(.+)"" to be (checked|unchecked)")]
        public async Task WhenIUpdateTheComponentFieldLabelledToBe(string labelText, string state)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

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
            await _web.ExpectStatusCode();
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

            Assert.HtmlEqual(expectedHtml, innerHtml, _outputHelper.WriteLine);
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

        [Then(@"the component outer element class should contain ""([^""]*)""")]
        public async Task ThenTheComponentOuterElementClassShouldContain(string expectedClass)
        {
            var component = await ComponentShouldExistAsync();
            var @classes = await component.AttributeAsync("class");

            Assert.Contains(expectedClass, @classes.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        [Then(@"the component outer element should match the selector ""(.+)""")]
        public async Task ThenTheComponentOuterElementShouldMatchTheSelector(string selector, string selectorToMatch)
        {
            var component = await ComponentShouldExistAsync();

            var matches = await component.MatchesAsync(selectorToMatch);
            Assert.True(matches, $@"The element did not match ""{selectorToMatch}"".");
        }

        [Then(@"the component outer element should have the attribute ""(.*)"" set to ""(.*)""")]
        public async Task ThenTheComponentOuterElementShouldHaveTheAttribute(string attribute, string expectedValue)
        {
            var component = await ComponentShouldExistAsync();

            var @class = await component.AttributeAsync(attribute);
            Assert.Equal(expectedValue, @class);
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

        [Then(@"the element ""(.+)"" within the component class should contain ""([^""]*)""")]
        public async Task ThenTheElementWithinTheComponentClassShouldContain(string selector, string expectedClass)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var @classes = await element.AttributeAsync("class");

            Assert.Contains(expectedClass, @classes.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        [Then(@"the element ""(.+)"" within the component should have the text content ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheTextContent(string selector, string expectedTextContent)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var textContent = await element.TextContentAsync();

            Assert.Equal(expectedTextContent.Trim(), textContent.Trim());
        }

        [Then(@"the element ""(.+)"" within the component should have the immediate text content ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheImmediateTextContent(string selector, string expectedTextContent)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var textContent = await element.ImmediateTextContentAsync();

            Assert.Equal(expectedTextContent.Trim(), textContent.Trim());
        }

        [Then(@"the element ""(.+)"" within the component should have the inner HTML ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheInnerHtml(string selector, string expectedHtml)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var innerHtml = await element.InnerHtmlAsync();

            Assert.HtmlEqual(expectedHtml, innerHtml, _outputHelper.WriteLine);
        }

        [Then(@"the element ""(.+)"" within the component should have the href ""([^""]*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheHref(string selector, string expectedHref)
        {
            var element = await ComponentElementShouldExistAsync(selector);
            var href = await element.AttributeAsync("href");

            Assert.Equal(expectedHref.Trim(), href.Trim());
        }

        [Then(@"the element ""(.+)"" within the component should match the selector ""(.+)""")]
        public async Task ThenTheElementWithinTheComponentShouldMatchTheSelector(string selector, string selectorToMatch)
        {
            var element = await ComponentElementShouldExistAsync(selector);

            var matches = await element.MatchesAsync(selectorToMatch);
            Assert.True(matches, $@"The element did not match ""{selectorToMatch}"".");
        }

        [Then(@"the element ""(.*)"" within the component should have the attribute ""(.*)"" set to ""(.*)""")]
        public async Task ThenTheElementWithinTheComponentShouldHaveTheAttribute(string selector, string attribute, string expectedValue)
        {
            var element = await ComponentElementShouldExistAsync(selector);

            var @class = await element.AttributeAsync(attribute);
            Assert.Equal(expectedValue, @class);
        }

        [Then(@"the elements ""(.+)"" within the component should total (.*)")]
        public async Task ThenTheElementsWithinTheComponentShouldTotal(string selector, int expectedCount)
        {
            var component = await ComponentShouldExistAsync();

            var elements = component.Elements($":scope {selector}");
            await elements.ShouldHaveCountAsync(expectedCount, actual => $"Expected {expectedCount} elements but found {actual}");
        }

        [Then(@"the elements ""(.+)"" within the component should have the text contents:")]
        public async Task ThenTheElementsWithinTheComponentShouldHaveTheTextContents(string selector, Table content)
        {
            int index = 0;
            var component = await ComponentShouldExistAsync();
            var elements = component.Elements($":scope {selector}");
            await elements.ShouldHaveCountAsync(content.RowCount, actual => $"Expected {content.RowCount} elements but found {actual}");
            var textContents = await elements.TextContentsAsync();

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), textContents[index].Trim());
                index++;
            }
        }

        [Then(@"the elements ""(.+)"" within the component should have the hrefs:")]
        public async Task ThenTheElementsWithinTheComponentShouldHaveTheHrefs(string selector, Table content)
        {
            int index = 0;
            var component = await ComponentShouldExistAsync();
            var elements = component.Elements($":scope {selector}");
            await elements.ShouldHaveCountAsync(content.RowCount, actual => $"Expected {content.RowCount} elements but found {actual}");
            var hrefs = await elements.AttributeValuesAsync("href");

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), hrefs[index].Trim());
                index++;
            }
        }

        [Then(@"the elements ""(.+)"" within the component should have the classes:")]
        public async Task ThenTheElementsWithinTheComponentShouldHaveTheClasses(string selector, Table content)
        {
            int index = 0;
            var component = await ComponentShouldExistAsync();
            var elements = component.Elements($":scope {selector}");
            await elements.ShouldHaveCountAsync(content.RowCount, actual => $"Expected {content.RowCount} elements but found {actual}");
            var classes = await elements.AttributeValuesAsync("class");

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First(), classes[index].Trim());
                index++;
            }
        }

        [Then(@"the elements ""(.+)"" within the component should all have the class ""(.+)""")]
        public async Task ThenTheElementsWithinTheComponentShouldAllHaveTheClass(string selector, string expectedClass)
        {
            var component = await ComponentShouldExistAsync();
            var elements = component.Elements($":scope {selector}");
            var classes = await elements.AttributeValuesAsync("class");

            Assert.All(classes, c => Assert.Equal(expectedClass, c));
        }

        [Then(@"the component field labelled ""(.+)"" should have the value ""(.*)""")]
        public async Task ThenTheComponentFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");
           
            var value = await field.ValueAsync();
            Assert.Equal(expectedValue, value);
        }

        [Then(@"the component field labelled ""(.+)"" should be (checked|unchecked)")]
        public async Task ThenTheComponentFieldLabelledShouldBeUnchecked(string labelText, string state)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");
            
            var isChecked = await field.IsCheckedAsync();
            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the component field labelled ""(.+)"" should match the JSON string ""(.*)""")]
        public async Task ThenTheComponentFieldLabelledShouldMatchTheJSONString(string labelText, string expectedValue)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actualValue = await field.ValueAsync();

            var normalizedExpected = JsonHelper.Serialize(JsonHelper.DeserializeOrNull<object>(expectedValue));
            var normalizedActual = JsonHelper.Serialize(JsonHelper.DeserializeOrNull<object>(actualValue));
            Assert.Equal(normalizedExpected, normalizedActual);
        }

        [Then(@"the input element of the component field labelled ""(.+)"" should match the selector ""(.+)""")]
        public async Task ThenTheInputElementOfTheComponentFieldLabelledShouldMatchTheSelector(string labelText, string selector)
        {
            var component = await ComponentShouldExistAsync();

            var field = await component.ElementByLabel(labelText)
                .ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var matches = await field.MatchesAsync(selector);

            Assert.True(matches, $@"The associated input element for the label ""{labelText}"" did not match ""{selector}"".");
        }

        private async Task<IElementDriver> ComponentShouldExistAsync()
        {
            await ThenThereShouldBeNoErrors();

            var testWrapper = await _web.CurrentPage.ElementAsync("#test");
            await testWrapper.ShouldExistAsync($@"Could not find an element with the selector ""#test"".");
            
            var component = await testWrapper.Element(":scope > div > *")
                .ShouldExistAsync($@"Could not find the component's outer element on the page.");

            return component;
        }

        private async Task ComponentShouldNotExistAsync()
        {
            await ThenThereShouldBeNoErrors();

            var testWrapper = await _web.CurrentPage.ElementAsync("#test");
            await testWrapper.ShouldExistAsync($@"Could not find an element with the selector ""#test"".");

            Assert.NotNull(testWrapper, $@"Could not find an element with the selector ""#test"".");

            await testWrapper.Element(":scope > div > *")
                .ShouldNotExistAsync($@"Found the component's outer element on the page.");
        }

        private async Task<IElementDriver> ComponentElementShouldExistAsync(string selector)
        {
            var component = await ComponentShouldExistAsync();

            var element = component.Element($":scope {selector}");
            await element.ShouldHaveCountAsync(1, (expected, actual) => $@"Found {actual} elements within the component with the selector ""{selector}"", expected {expected}.");
            await element.ShouldExistAsync($@"Could not find an element within the component with the selector ""{selector}"".");

            return element;
        }

        private async Task ComponentElementShouldNotExistAsync(string selector)
        {
            var component = await ComponentShouldExistAsync();

            await component.Element($":scope {selector}")
                .ShouldNotExistAsync($@"Found an element within the component with the selector ""{selector}"".");
        }
    }
}
