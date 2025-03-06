using ASP.Web.FunctionalTests.Drivers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow.Infrastructure;
using YamlDotNet.Core.Tokens;

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
            var browserTitle = await _web.CurrentPage.TitleAsync();

            Assert.Equal(expected == "Analyse school performance" ? expected : (expected + " | Analyse school performance"), browserTitle);

            var pageTitleElement = await _web.CurrentPage.ElementAsync("#app-page-title");
            var pageTitle = await pageTitleElement.TextContentAsync();

            Assert.Equal(expected.Trim().ReplaceLineEndings(" "), pageTitle.Trim().ReplaceLineEndings(" "), "Page titles differ", ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true, ignoreAllWhiteSpace: true);
        }

        [Then(@"the page subtitle should be ""(.*)""")]
        public async Task ThenThePageSubtitleShouldBe(string expected)
        {
            var subtitleElement = await _web.CurrentPage.ElementAsync("#app-page-subtitle");
            var subtitle = await subtitleElement.TextContentAsync();

            Assert.Equal(expected.Trim().ReplaceLineEndings(" "), subtitle.Trim().ReplaceLineEndings(" "), "Page subtitles differ", ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Then(@"the sub-page title should be ""(.+)"" with no caption")]
        public async Task ThenTheSubpageTitleShouldBe(string expected)
        {
            var subpageTitleElement = await _web.CurrentPage.ElementAsync("#app-subpage-title");
            var subpageTitle = await subpageTitleElement.TextContentAsync();

            Assert.Equal(expected.Trim().ReplaceLineEndings(" "), subpageTitle.Trim().ReplaceLineEndings(" "), "Sub-page titles differ", ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
            var captionElement = await _web.CurrentPage.ElementAsync("#app-subpage-title-caption");
            await captionElement.ShouldNotExistAsync("Expected the subpage title not to have a caption");
        }

        [Then(@"the sub-page title should be ""(.+)"" with caption ""(.*)""")]
        public async Task ThenTheSubpageTitleShouldBe(string expectedTitle, string expectedCaption)
        {
            var subpageTitleElement = await _web.CurrentPage.ElementAsync("#app-subpage-title");
            var captionElement = await _web.CurrentPage.ElementAsync("#app-subpage-title-caption");
            var subpageTitle = await subpageTitleElement.TextContentAsync();
            var caption = await captionElement.TextContentAsync();

            Assert.Equal(expectedCaption.Trim().ReplaceLineEndings(" "), caption.Trim().ReplaceLineEndings(" "), "Sub-page title captions differ", ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true, ignoreAllWhiteSpace: true);
            Assert.Equal($"{expectedCaption.Trim().ReplaceLineEndings(" ")} {expectedTitle.Trim().ReplaceLineEndings(" ")}", subpageTitle.Trim().ReplaceLineEndings(" "), "Sub-page titles differ", ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true, ignoreAllWhiteSpace: true);
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

            var isTextContent = valueType == "text content";
            Assert.Equal(expectedValue.Trim().ReplaceLineEndings(" "), value.Trim().ReplaceLineEndings(" "), ignoreLineEndingDifferences: isTextContent, ignoreWhiteSpaceDifferences: isTextContent);
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
                var isTextContent = valueTypes == "text contents";
                Assert.Equal(item.Values.First().Trim().ReplaceLineEndings(" "), values[index].Trim().ReplaceLineEndings(" "), ignoreLineEndingDifferences: isTextContent, ignoreWhiteSpaceDifferences: isTextContent);
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

            var isTextContent = valueType == "text content";
            Assert.All(value, c => Assert.Equal(expectedValue.Trim().ReplaceLineEndings(" "), c.Trim().ReplaceLineEndings(" "), ignoreLineEndingDifferences: isTextContent, ignoreWhiteSpaceDifferences: isTextContent));
        }

        [Then("the top navigation should be:")]
        public async Task ThenTheTopNavigationShouldBe(Table expectedListItems)
        {
            await AssertOnListOfNavigationItems("#navigation > li", "Top navigation item", expectedListItems);
        }

        [Then("the breadcrumb trail should be:")]
        public async Task ThenTheBreadcrumbTrailShouldBe(Table expectedListItems)
        {
            await AssertOnListOfNavigationItems("[data-testid='breadcrumbs'] > li", "Breadcrumb", expectedListItems);
        }
        
        [Then("the landing page cards should be:")]
        public async Task ThenTheLandingPageCardsShouldBe(Table cards)
        {
            await AssertOnLandingPageCards(".app-card", "Landing page card", cards);
        }

        [Then("the sub-navigation should be:")]
        public async Task ThenTheSubnavigationShouldBe(Table expectedListItems)
        {
            await AssertOnListOfNavigationItems("[data-testid='sub-navigation'] > li", "Sub-navigation item", expectedListItems);
        }

        [Then("the side navigation should be:")]
        public async Task ThenTheSideNavigationShouldBe(Table expectedListItems)
        {
            await AssertOnListOfNavigationItems("[data-testid='side-navigation'] > li", "Side navigation item", expectedListItems);
        }

        [Then("the available download formats should be:")]
        public async Task ThenTheAvailableDownloadFormatsShouldBe(Table expectedListItems)
        {
            await AssertOnListOfNavigationItems("[data-testid='available-downloads-formats'] > li", "Download format", expectedListItems);
        }
        
        [Then("the linked schools links should be:")]
        public async Task ThenTheLinkedEstablishmentDescriptionShouldBe(Table expectedListItems)
        {
            await AssertOnListOfLinks("[data-testid='linked-school'] li", "Linked school", expectedListItems);
        }

        [Then(@"the pagination summary should be ""(.+)""")]
        public async Task ThenThePaginationSummaryShouldBe(string summary)
        {
            await ThenTheElementShouldHaveTheValue("element", ".pagination-container-p-text", "text content", summary);
        }

        [Then("the pagination links should be:")]
        public async Task ThenThePaginationLinksShouldBe(Table expectedListItems)
        {
            await AssertOnListOfLinks(".govuk-pagination .govuk-pagination__prev, .govuk-pagination .govuk-pagination__item, .govuk-pagination .govuk-pagination__next", "Pagination link", expectedListItems);
        }

        [Then("the pagination links should be empty")]
        public async Task ThenThePaginationLinksShouldBeEmpty()
        {
            var elements = await _web.CurrentPage.ElementsAsync(".govuk-pagination .govuk-pagination__prev, .govuk-pagination .govuk-pagination__item, .govuk-pagination .govuk-pagination__next");
            var actualItemCount = await elements.CountAsync();
            Assert.Equal(0, actualItemCount, $"Expected pagination links to be empty, but found {actualItemCount} items");
        }

        [Then("the listings should be:")]
        public async Task ThenTheListingsShouldBe(Table expectedListItems)
        {
            await AssertOnListOfLinks(".app-listing li", "Listing", expectedListItems, async (element, textElement, row, item) =>
            {
                var remainingRowKeys = row.Keys.Except(["Text", "Url", "Index"], StringComparer.InvariantCultureIgnoreCase);
                foreach (var key in remainingRowKeys)
                {
                    if (TryGetRowKey(key, row, out var expectedValue))
                    {
                        var value = element.Element($".app-listing-{key.ToLowerInvariant()}");
                        var actualValue = (await value.TextContentAsync()).Trim();
                        Assert.Equal(expectedValue, actualValue, $@"Expected {item} {key} to be ""{expectedValue}"" but was ""{actualValue}""");
                    }
                }
            });
        }

        private async Task AssertOnListOfNavigationItems(string selector, string itemName, Table expectedListItems)
        {
            await AssertOnListOfLinks(selector, itemName, expectedListItems, async (element, textElement, row, item) => {
                if (TryGetRowKey("Current Page", row, out var currentString))
                {
                    var expectedIsCurrent = bool.TryParse(currentString, out var val) && val;
                    var ariaCurrent = (await textElement.AttributeAsync("aria-current")).Trim();

                    if (expectedIsCurrent)
                    {
                        Assert.Equal("page", ariaCurrent, $"Expected {item} aria-current attribute to be \"page\" but was \"{ariaCurrent}\"");
                    }
                    else
                    {
                        Assert.NotEqual("page", ariaCurrent, $"Expected {item} aria-current attribute not to be \"page\" but was \"{ariaCurrent}\"");
                    }
                }
            });
        }

        private async Task AssertOnListOfLinks(string selector, string itemName, Table expectedListItems, Func<IElementDriver, IElementDriver, TableRow, string, Task>? extraItemAssertions = null)
        {
            var elements = await _web.CurrentPage.ElementsAsync(selector);
            var rowsAreIndexed = expectedListItems.ContainsColumn("Index");
            if (!rowsAreIndexed)
            {
                var actualItemCount = await elements.CountAsync();
                Assert.Equal(expectedListItems.RowCount, actualItemCount, $"Expected {itemName} count to be {expectedListItems.RowCount}, but was {actualItemCount}");
            }

            for (var i = 0; i < expectedListItems.RowCount; i++)
            {
                var row = expectedListItems.Rows[i];
                var index = rowsAreIndexed && row.TryGetValue("Index", out string ix) ? int.Parse(ix) - 1 : i;
                var element = elements.ElementAt(index);
                var item = $"{itemName} {index + 1}";
                await element.ShouldExistAsync($"{item} does not exist");

                var link = element.Element("a");
                var linkExists = await link.ExistsAsync();
                var textElement = linkExists ? link : element;

                if (TryGetRowKey("Link Text", row, out var expectedText))
                {
                    var actualText = (await textElement.TextContentAsync()).Trim();
                    Assert.Equal(expectedText, actualText, $@"Expected {item} Text to be ""{expectedText}"" but was ""{actualText}""");
                }

                if (TryGetRowKey("Text Content", row, out var expectedTextContent))
                {
                    var actualTextContent = (await element.TextContentAsync()).Trim();
                    Assert.Equal(expectedTextContent, actualTextContent, $@"Expected {item} Text Content to be ""{expectedTextContent}"" but was ""{actualTextContent}""");
                }

                if (TryGetRowKey("Url", row, out var expectedHref) && !string.IsNullOrWhiteSpace(expectedHref))
                {
                    await link.ShouldExistAsync($"Link element for {item} does not exist");
                    var actualHref = (await link.AttributeAsync("href")).Trim();
                    Assert.Equal(expectedHref, actualHref, $@"Expected {item} Url to be ""{expectedHref}"" but was ""{actualHref}""");
                }

                if(extraItemAssertions != null)
                {
                    await extraItemAssertions(element, textElement, row, item);
                }
            }
        }

        private async Task AssertOnLandingPageCards(string selector, string itemName, Table expectedListItems)
        {
            var elements = await _web.CurrentPage.ElementsAsync(selector);
            var rowsAreIndexed = expectedListItems.ContainsColumn("Index");
            if (!rowsAreIndexed)
            {
                var actualItemCount = await elements.CountAsync();
                Assert.Equal(expectedListItems.RowCount, actualItemCount, $"Expected {itemName} count to be {expectedListItems.RowCount}, but was {actualItemCount}");
            }

            for (var i = 0; i < expectedListItems.RowCount; i++)
            {
                var row = expectedListItems.Rows[i];
                var index = rowsAreIndexed && row.TryGetValue("Index", out string ix) ? int.Parse(ix) - 1 : i;
                var element = elements.ElementAt(index);
                var item = $"{itemName} {index + 1}";
                await element.ShouldExistAsync($"{item} does not exist");

                var linkTitleElement = element.Element(":scope > .app-card-container > h2 > a");
                var contentElement = element.Element(":scope > .app-card-container > p.govuk-body");

                if (TryGetRowKey("Title", row, out var expectedTitle))
                {
                    await linkTitleElement.ShouldExistAsync($"Link element for {item} does not exist");
                    var actualTitle = (await linkTitleElement.TextContentAsync()).Trim();
                    Assert.Equal(expectedTitle, actualTitle, $@"Expected {item} Title to be ""{expectedTitle}"" but was ""{actualTitle}""");
                }

                if (TryGetRowKey("Url", row, out var expectedUrl))
                {
                    await linkTitleElement.ShouldExistAsync($"Link element for {itemName} {i + 1} does not exist");
                    var actualUrl = (await linkTitleElement.AttributeAsync("href")).Trim();
                    Assert.Equal(expectedUrl, actualUrl, $@"Expected {item} Url to be ""{expectedUrl}"" but was ""{actualUrl}""");
                }

                if (TryGetRowKey("Content", row, out var expectedContent))
                {
                    await contentElement.ShouldExistAsync($"Content element for {item} does not exist");
                    var actualContent = (await contentElement.TextContentAsync()).Trim();
                    Assert.Equal(expectedContent, actualContent, $@"Expected {item} Content to be ""{expectedContent}"" but was ""{actualContent}""");
                }
            }
        }

        private bool TryGetRowKey(string key, TableRow row, [NotNullWhen(true)] out string? value)
        {
            if(row.ContainsKey(key))
            {
                value = row[key].Trim();
                return true;
            }

            if (row.ContainsKey(key.ToLowerInvariant()))
            {
                value = row[key.ToLowerInvariant()].Trim();
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