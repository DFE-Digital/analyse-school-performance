using ASP.Web.FunctionalTests.Drivers;
using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow.Infrastructure;

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
        public async Task ThenTheTopNavigationShouldBe(Table navigationItems)
        {
            await AssertNavigation("#navigation", "Top navigation item", navigationItems);
        }

        [Then("the breadcrumb trail should be:")]
        public async Task ThenTheBreadcrumbTrailShouldBe(Table breadcrumbs)
        {
            await AssertNavigation("[data-testid='breadcrumbs']", "Breadcrumb", breadcrumbs);
        }
        
        [Then("the landing page cards should be:")]
        public async Task ThenTheLandingPageCardsShouldBe(Table cards)
        {
            await AssertLandingPageCards("Landing page card", cards);
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
        
        [Then("the linked establishment description should be:")]
        public async Task ThenTheLinkedEstablishmentDescriptionShouldBe(Table navigationItems)
        {
            await AssertLinkedEstablishmentDescriptionNavigation("[data-testid='linked-school'] li", "Linked establishment description", navigationItems);
        }

        private async Task AssertNavigation(string selector, string itemName, Table navigationItems)
        {
            var elements = await _web.CurrentPage.ElementsAsync($"{selector} > li");
            var actualItemCount = await elements.CountAsync();
            Assert.Equal(navigationItems.RowCount, actualItemCount, $"Expected {itemName} count to be {navigationItems.RowCount}, but was {actualItemCount}");
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
        
        private async Task AssertLandingPageCards(string itemName, Table cards)
        {
            for (var i = 0; i < cards.RowCount; i++)
            {
                var row = cards.Rows[i];
        
                // Select each card using nth-child
                var element = await _web.CurrentPage.ElementAsync($".app-card:nth-child({i + 1})");
                await element.ShouldExistAsync($"Card {i + 1} does not exist");
                
                var linkTitleElement = element.Element(":scope > .app-card-container > h2 > a");
                var contentElement = element.Element(":scope > .app-card-container > p.govuk-body");

                if (TryGetRowKey("Title", row, out var expectedTitle))
                {
                    await linkTitleElement.ShouldExistAsync($"Link element for {itemName} {i + 1} does not exist");
                    var actualText = await linkTitleElement.TextContentAsync();
                    Assert.Equal(expectedTitle, actualText.Trim());
                }

                if (TryGetRowKey("Url", row, out var expectedUrl))
                {
                    var rowName = row.ContainsKey("Title") ? $"(\"{row["Title"]}\")" : "";
                    await linkTitleElement.ShouldExistAsync($"Link element for {itemName} {i + 1} {rowName} does not exist");
                    var actualHref = await linkTitleElement.AttributeAsync("href");
                    Assert.Equal(expectedUrl, actualHref.Trim());
                }

                if (TryGetRowKey("Content", row, out var expectedContent))
                {
                    await contentElement.ShouldExistAsync($"Content element for {itemName} {i + 1} does not exist");
                    var actualContent = await contentElement.TextContentAsync();
                    Assert.Equal(expectedContent, actualContent.Trim());
                }
            }
        }
        
        private async Task AssertLinkedEstablishmentDescriptionNavigation(string selector, string itemName, Table navigationItems)
        {
            var elements = await _web.CurrentPage.ElementsAsync(selector);
            var actualItemCount = await elements.CountAsync();
            Assert.Equal(navigationItems.RowCount, actualItemCount, $"Expected {itemName} count to be {navigationItems.RowCount}, but was {actualItemCount}");

            for (var i = 0; i < navigationItems.RowCount; i++)
            {
                var row = navigationItems.Rows[i];
                var element = await _web.CurrentPage.ElementAsync($"{selector}:nth-child({i + 1})");
                await element.ShouldExistAsync($"{itemName} {i + 1} does not exist");

                if (TryGetRowKey("Text", row, out var expectedText))
                {
                    var actualText = await element.TextContentAsync();
                    Assert.Equal(expectedText.Trim(), actualText.Trim());
                }

                if (TryGetRowKey("Href", row, out var expectedHref) && !string.IsNullOrWhiteSpace(expectedHref))
                {
                    element = element.Element("a");
                    var rowName = row.TryGetValue("text", out var value) ? $"(\"{value}\")" : "";
                    await element.ShouldExistAsync($"Link element for {itemName} {i + 1} {rowName} does not exist");
                    var actualHref = await element.AttributeAsync("href");
                    Assert.Equal(expectedHref, actualHref.Trim());
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