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
        private readonly ScenarioContext _scenarioContext;

        public AspWebStepDefinitions(IWebDriver web, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            _web = web;
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
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

        [When(@"the application returns a 404 with error message ""([^""]*)""")]
        public async Task TheApplicationReturns404WithErrorMessage(string errorMessage)
        {
            await _web.NavigateAsync($"/error-test/page-not-found-error/{errorMessage}");
        }

        [When(@"I update the element ""(.+)"" to be (checked|unchecked)")]
        public async Task WhenIUpdateTheElementToBe(string selector, string state)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            await element.SetCheckedAsync(state == "checked");
        }

        [When(@"I update the textbox ""(.+)"" to have the value ""([^""]*)""")]
        public async Task WhenIUpdateTheTextBoxToHaveTheValue(string selector, string value)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            await element.SetValueAsync(value);
        }

        [When(@"I click the button ""(.+)""")]
        public async Task WhenIClickTheButton(string elementSelector)
        {
            var button = await _web.Element(elementSelector);
            await button.ShouldExistAsync($@"Could not find an element with the selector ""{elementSelector}"".");

            await button.ClickAsync();
        }

        [When(@"I remember the (text content|tag name|class|value|href|outer HTML|inner HTML) of (element|textbox|input|hidden input|button) ""(.+)"" as <([^>]+)>")]
        public async Task WhenIRememberTheElementValueAs(string valueType, string elementType, string selector, string variableName)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var value = valueType switch {
                "text content" => await element.TextContentAsync(),
                "tag name" => await element.TagNameAsync(),
                "class" => await element.AttributeAsync("class"),
                "value" => await element.ValueAsync(),
                "href" => await element.AttributeAsync("href"),
                "outer HTML" => await element.OuterHtmlAsync(),
                "inner HTML" => await element.InnerHtmlAsync(),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            _scenarioContext[variableName] = value.Trim();
        }

        [Then(@"I should get a (.*) response")]
        public async Task ThenIShouldGetAResponse(int statusCode)
        {
            _web.ExpectedStatusCode = statusCode;

            if ((int)_web.Status != 200)
            {
                var pageContent = await _web.PageContentAsync();
                _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
            }

            Assert.Equal((HttpStatusCode)statusCode, _web.Status);
        }

        [Then(@"The path should match ((?:/.*)+)")]
        public void ThePathShouldMatch(string path)
        {
            Assert.Equal(_web.BaseAddress + path, _web.Path);
        }

        [Then(@"the page title should be ""([^""]*)""")]
        public async Task ThenThePageTitleShouldBe(string expected)
        {
            var actual = await _web.PageTitleAsync();

            Assert.Equal(expected, actual);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should (exist|not exist)")]
        public async Task ThenTheElementShouldExist(string elementType, string selector, string criteria)
        {
            var element = await _web.Element(selector);

            if(criteria == "exist")
            {
                await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");
            }
            else
            {
                await element.ShouldNotExistAsync($@"Found an element with the selector ""{selector}"".");
            }
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should match the selector ""(.+)""")]
        public async Task ThenTheElementShouldMatchTheSelector(string elementType, string selector, string selectorToMatch)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var matches = await element.MatchesAsync(selectorToMatch);
            AssertWithMessage.True(matches, $@"The element did not match ""{selectorToMatch}"".");
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the (outer HTML|inner HTML) <(.+)>")]
        public async Task ThenTheElementShouldHaveTheHtmlFromVariable(string elementType, string selector, string valueType, string variableName)
        {
            var expectedValue = ResolveVariable(variableName);

            await ThenTheElementShouldHaveTheHtmlMultiline(elementType, selector, valueType, expectedValue);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the (outer HTML|inner HTML):")]
        public async Task ThenTheElementShouldHaveTheHtmlMultiline(string elementType, string selector, string valueType, string expectedValue)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var value = valueType switch {
                "outer HTML" => await element.OuterHtmlAsync(),
                "inner HTML" => await element.InnerHtmlAsync(),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            AssertHtml.Equal(expectedValue.Trim(), value.Trim(), _outputHelper.WriteLine);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" class should contain ""([^""]*)""")]
        public async Task ThenTheElementClassShouldContain(string elementType, string selector, string expectedClass)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var @classes = await element.AttributeAsync("class");

            Assert.Contains(expectedClass, @classes.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the (text content|tag name|class|value|href) <(.+)>")]
        public async Task ThenTheElementShouldHaveTheValueFromVariable(string elementType, string selector, string valueType, string variableName)
        {
            var expectedValue = ResolveVariable(variableName);

            await ThenTheElementShouldHaveTheValue(elementType, selector, valueType, expectedValue);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the (text content|tag name|class|value|href) ""(.*)""")]
        public async Task ThenTheElementShouldHaveTheValue(string elementType, string selector, string valueType, string expectedValue)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var value = valueType switch {
                "text content" => await element.TextContentAsync(),
                "tag name" => await element.TagNameAsync(),
                "class" => await element.AttributeAsync("class"),
                "value" => await element.ValueAsync(),
                "href" => await element.AttributeAsync("href"),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            AssertHtml.Equal(expectedValue.Trim(), value.Trim(), _outputHelper.WriteLine);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should be (checked|unchecked)")]
        public async Task ThenTheElementShouldBe(string elementType, string selector, string state)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var isChecked = await element.IsCheckedAsync();
            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the attribute ""(.*)"" set to ""(.*)""")]
        public async Task ThenTheElementShouldHaveTheAttribute(string elementType, string selector, string attribute, string expectedValue)
        {
            var element = await _web.Element(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var @class = await element.AttributeAsync(attribute);
            Assert.Equal(expectedValue, @class);
        }

        [Then(@"the field labelled ""(.+)"" should have the value ""(.*)""")]
        public async Task ThenTheFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var field = await _web.ElementByLabel(labelText);
            await field.ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actualValue = await field.ValueAsync();
            Assert.Equal(expectedValue, actualValue);
        }

        [Then(@"the (elements|textboxes|inputs|hidden inputs|buttons) ""(.+)"" should total (.*)")]
        public async Task ThenTheElementsShouldTotal(string elementType, string selector, int expectedCount)
        {
            var elements = await _web.Elements(selector);
            await elements.ShouldHaveCountAsync(expectedCount, actual => $"Expected {expectedCount} elements but found {actual}");
        }

        [Then(@"the (elements|textboxes|inputs|hidden inputs|buttons) ""(.+)"" should have the (text contents|tag names|classes|values|hrefs):")]
        public async Task ThenTheElementsShouldHaveTheTextContents(string elementType, string selector, string valueTypes, Table content)
        {
            int index = 0;
            var elements = await _web.Elements(selector);
            await elements.ShouldHaveCountAsync(content.RowCount, actual => $"Expected {content.RowCount} elements but found {actual}");
            
            var values = valueTypes switch {
                "text contents" => await elements.TextContentsAsync(),
                "tag name" => await elements.TagNamesAsync(),
                "classes" => await elements.AttributeValuesAsync("class"),
                "value" => await elements.ValuesAsync(),
                "hrefs" => await elements.AttributeValuesAsync("href"),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            foreach (var item in content.Rows)
            {
                Assert.Equal(item.Values.First().Trim(), values[index].Trim());
                index++;
            }
        }

        [Then(@"the (elements|textboxes|inputs|hidden inputs|buttons) ""(.+)"" should all have the (text content|tag name|class|value|href) ""(.+)""")]
        public async Task ThenTheElementsShouldAllHaveTheClass(string elementType, string selector, string valueType, string expectedValue)
        {
            var elements = await _web.Elements(selector);

            var value = valueType switch {
                "text content" => await elements.TextContentsAsync(),
                "tag name" => await elements.TagNamesAsync(),
                "class" => await elements.AttributeValuesAsync("class"),
                "value" => await elements.ValuesAsync(),
                "href" => await elements.AttributeValuesAsync("href"),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            var classes = await elements.AttributeValuesAsync("class");
            Assert.All(classes, c => Assert.Equal(expectedValue, c));
        }

        private string ResolveVariable(string variableName)
        {
            if (!_scenarioContext.ContainsKey(variableName))
            {
                Assert.Fail($@"Variable ""{variableName}"" was not set. Did you forget a ""When I remember ... "" step? ;)");
            }

            return (string)_scenarioContext[variableName];
        }
    }
}