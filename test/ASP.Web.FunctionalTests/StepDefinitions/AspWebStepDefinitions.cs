using AngleSharp.Dom;
using ASP.Test.Core;
using ASP.Web.FunctionalTests.Drivers;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    // Step definitions for test scenarios that interact with the ASP web application
    [Binding]
    public class AspWebStepDefinitions
    {
        private const string HTTP_HEADER = @"""([^:""]+): ([^:""]+)""";

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
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            await element.SetCheckedAsync(state == "checked");
        }

        [When(@"I update the textbox ""(.+)"" to have the value ""([^""]*)""")]
        public async Task WhenIUpdateTheTextBoxToHaveTheValue(string selector, string value)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            await element.SetValueAsync(value);
        }

        [When(@"I click the button ""(.+)""")]
        public async Task WhenIClickTheButton(string elementSelector)
        {
            var button = await _web.CurrentPage.ElementAsync(elementSelector);
            await button.ShouldExistAsync($@"Could not find an element with the selector ""{elementSelector}"".");

            await button.ClickAsync();
        }

        [When(@"I remember the (text content|tag name|class|value|href|outer HTML|inner HTML) of (element|textbox|input|hidden input|button) ""(.+)"" as <([^>]+)>")]
        public async Task WhenIRememberTheElementValueAs(string valueType, string elementType, string selector, string variableName)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
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
            await _web.ExpectStatusCode();
        }

        [Then($@"the response should include the header {HTTP_HEADER}")]
        public void ThenTheResponseShouldIncludeTheHeader(string name, string value)
        {
            Assert.Contains(name, value, _web.Headers, StringComparison.InvariantCultureIgnoreCase);
        }

        [Then(@"the response should include the headers:")]
        public void ThenTheResponseShouldIncludeTheHeaders(Table table)
        {
            foreach (var row in table.Rows)
            {
                Assert.Contains(row["Key"], row["Value"], _web.Headers, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        [Then(@"the path should be ((?:/.*)+)")]
        public async Task ThePathShouldBe(string path)
        {
            await _web.ExpectStatusCode();
            Assert.Equal(_web.BaseAddress + path, _web.CurrentPage.Path);
        }

        [Then(@"the page title should be ""(.*)""")]
        public async Task ThenThePageTitleShouldBe(string expected)
        {
            var actual = await _web.CurrentPage.TitleAsync();

            Assert.Equal(expected, actual);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should (exist|not exist)")]
        public async Task ThenTheElementShouldExist(string elementType, string selector, string criteria)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);

            if (criteria == "exist")
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
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var matches = await element.MatchesAsync(selectorToMatch);
            Assert.True(matches, $@"The element did not match ""{selectorToMatch}"".");
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
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var value = valueType switch {
                "outer HTML" => await element.OuterHtmlAsync(),
                "inner HTML" => await element.InnerHtmlAsync(),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            Assert.HtmlEqual(expectedValue.Trim(), value.Trim(), _outputHelper.WriteLine);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" class should contain ""([^""]*)""")]
        public async Task ThenTheElementClassShouldContain(string elementType, string selector, string expectedClass)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
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
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var value = valueType switch {
                "text content" => await element.TextContentAsync(),
                "tag name" => await element.TagNameAsync(),
                "class" => await element.AttributeAsync("class"),
                "value" => await element.ValueAsync(),
                "href" => await element.AttributeAsync("href"),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            Assert.HtmlEqual(expectedValue.Trim(), value.Trim(), _outputHelper.WriteLine);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should be (checked|unchecked)")]
        public async Task ThenTheElementShouldBe(string elementType, string selector, string state)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var isChecked = await element.IsCheckedAsync();
            Assert.Equal(state == "checked", isChecked);
        }

        [Then(@"the (element|textbox|input|hidden input|button) ""(.+)"" should have the attribute ""(.*)"" set to ""(.*)""")]
        public async Task ThenTheElementShouldHaveTheAttribute(string elementType, string selector, string attribute, string expectedValue)
        {
            var element = await _web.CurrentPage.ElementAsync(selector);
            await element.ShouldExistAsync($@"Could not find an element with the selector ""{selector}"".");

            var @class = await element.AttributeAsync(attribute);
            Assert.Equal(expectedValue, @class);
        }


        [Then(@"the field labelled ""(.+)"" should have the value ""(.*)""")]
        public async Task ThenTheFieldLabelledShouldHaveTheValue(string labelText, string expectedValue)
        {
            var field = await _web.CurrentPage.ElementByLabelAsync(labelText);
            await field.ShouldExistAsync($@"Could not find the associated input for the label ""{labelText}"" (""for"" attribute missing or incorrect).");

            var actualValue = await field.ValueAsync();
            Assert.Equal(expectedValue, actualValue);
        }

        [Then(@"the (elements|textboxes|inputs|hidden inputs|buttons) ""(.+)"" should total (.*)")]
        public async Task ThenTheElementsShouldTotal(string elementType, string selector, int expectedCount)
        {
            var elements = await _web.CurrentPage.ElementsAsync(selector);
            await elements.ShouldHaveCountAsync(expectedCount, actual => $"Expected {expectedCount} elements but found {actual}");
        }

        [Then(@"the (elements|textboxes|inputs|hidden inputs|buttons) ""(.+)"" should have the (text contents|tag names|classes|values|hrefs):")]
        public async Task ThenTheElementsShouldHaveTheTextContents(string elementType, string selector, string valueTypes, Table content)
        {
            int index = 0;
            var elements = await _web.CurrentPage.ElementsAsync(selector);
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
            var elements = await _web.CurrentPage.ElementsAsync(selector);

            var value = valueType switch {
                "text content" => await elements.TextContentsAsync(),
                "tag name" => await elements.TagNamesAsync(),
                "class" => await elements.AttributeValuesAsync("class"),
                "value" => await elements.ValuesAsync(),
                "href" => await elements.AttributeValuesAsync("href"),
                _ => throw new NotImplementedException("We shouldn't have got here")
            };

            Assert.All(value, c => Assert.Equal(expectedValue, c));
        }

        [Then("the top navigation should be:")]
        public async Task ThenTheTopNavigationShouldBe(Table navigationItems)
        {
            await AssertNavigation("#navigation", "Top navigation item", navigationItems);
        }

        [Then("the breadcrumb trail should be:")]
        public async Task ThenTheBreadcrumbTrailShouldBe(Table breadcrumbs)
        {
            await AssertNavigation("[data-testid='breadcrumbs']", "Breadcrumb", breadcrumbs);
        }

        [Then("the sub-navigation should be:")]
        public async Task ThenTheSubnavigationShouldBe(Table navigationItems)
        {
            await AssertNavigation("[data-testid='sub-navigation']", "Sub-navigation item", navigationItems);
        }

        [Then("the side navigation should be:")]
        public async Task ThenTheSideNavigationShouldBe(Table navigationItems)
        {
            await AssertNavigation("[data-testid='side-navigation']", "Side navigation item", navigationItems);
        }

        [Then("the available download formats should be:")]
        public async Task ThenTheAvailableDownloadFormatsShouldBe(Table navigationItems)
        {
            await AssertNavigation("[data-testid='available-downloads-formats']", "Download format", navigationItems);
        }

        private async Task AssertNavigation(string selector, string itemName, Table navigationItems)
        {
            for (var i = 0; i < navigationItems.RowCount; i++)
            {
                var row = navigationItems.Rows[i];

                var element = await _web.CurrentPage.ElementAsync($"{selector} > li:nth-child({i + 1})");
                await element.ShouldExistAsync($"{itemName} {i + 1} does not exist");

                if (TryGetRowKey("Href", row, out var expectedHref) && !string.IsNullOrWhiteSpace(expectedHref))
                {
                    element = element.Element(":scope > a");
                    var rowName = row.ContainsKey("text") ? $"(\"{row["text"]}\")" : "";
                    await element.ShouldExistAsync($"Link element for {itemName} {i + 1} {rowName} does not exist");
                    var actualHref = await element.AttributeAsync("href");
                    Assert.Equal(expectedHref, actualHref.Trim());
                }

                if (TryGetRowKey("Text", row, out var expectedText))
                {
                    var actualText = await element.TextContentAsync();
                    Assert.Equal(expectedText, actualText.Trim());
                }

                if (TryGetRowKey("Current", row, out var currentString))
                {
                    var expectedIsCurrent = bool.TryParse(currentString, out var val) && val;
                    var ariaCurrent = await element.AttributeAsync("aria-current");
                    if (expectedIsCurrent)
                    {
                        Assert.Equal("page", ariaCurrent);
                    }
                    else
                    {
                        Assert.NotEqual("page", ariaCurrent);
                    }
                }
            }
        }

        private bool TryGetRowKey(string key, TableRow row, [NotNullWhen(true)] out string? value)
        {
            if(row.ContainsKey(key))
            {
                value = row[key];
                return true;
            }

            if (row.ContainsKey(key.ToLowerInvariant()))
            {
                value = row[key.ToLowerInvariant()];
                return true;
            }

            value = null;
            return false;
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