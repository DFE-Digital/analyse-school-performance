using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using ASP.Core.Helpers;
using ASP.Core.PageContent;
using ASP.Core.PageContent.Repository;
using ASP.Test.Core;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Repositories;
using ErrorOr;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public sealed partial class AspWebStepDefinitions
    {
        private static readonly Regex _spacesAfterClosingTag = new Regex(@">([\s\t\n]*)([^\s\t\n])", RegexOptions.Compiled);
        private static readonly Regex _spacesBeforeOpeningTag = new Regex(@"([^\s\t\n])([\s\t\n]*)<", RegexOptions.Compiled);
        private static readonly MinifyMarkupFormatter _minifier = new MinifyMarkupFormatter {
            ShouldKeepStandardElements = false,
            ShouldKeepAttributeQuotes = true,
            ShouldKeepEmptyAttributes = true,
            ShouldKeepImpliedEndTag = true,
            ShouldKeepComments = false
        };

        private readonly AspWeb _web;

        AspWebResponse? _response = null;
        IDocument _document = null;

        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(ISpecFlowOutputHelper outputHelper)
        {
            _web = new AspWeb();
            _outputHelper = outputHelper;
        }

        [BeforeScenario]
        public async Task ClearDownPageContent()
        {
            await _web.PageContentRepository.DeleteAll();
        }

        [Given(@"no page content exists")]
        public void GivenNoPageContentExists()
        {
        }

        [Given(@"page content ""([^""]*)"" exists:")]
        public async Task GivenPageContentExists(string id, string data)
        {
            await SetUpPageContent(id, data).SwitchFirst(
                data => {
                },
                e => AssertWithMessage.Failed(@$"Could not update page content with id ""{id}"": {e.Description}"));
        }

        [Given(@"I navigate to ((?:/.*)+)")]
        public async Task GivenINavigateTo(string path)
        {
            _response = await _web.GetAsync(path);
        }

        [When(@"I navigate to ((?:/.*)+)")]
        public async Task WhenINavigateTo(string path)
        {
            _response = await _web.GetAsync(path);
        }

        [When(@"I update the textbox ""([^""]*)"" to have the value ""([^""]*)""")]
        public void WhenIUpdateTheTextBoxToHaveTheValue(string selector, string value)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);
            input.Value = value;
        }

        [When(@"I submit the form ""([^""]*)""")]
        public async Task WhenISubmitTheForm(string formSelector)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(formSelector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{formSelector}"".");
            var form = Assert.IsAssignableFrom<IHtmlFormElement>(element);

            _document = await form.SubmitAsync();
        }

        [Then(@"I should get a (.*) response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            if (_document != null)
            {
                if (_document!.StatusCode != HttpStatusCode.OK)
                {
                    _outputHelper.WriteLine(_document!.Body.Html());
                }

                Assert.Equal(statusCode, (int)_document!.StatusCode);
            }
            else if (_response != null)
            {
                if (_response!.StatusCode != HttpStatusCode.OK)
                {
                    _outputHelper.WriteLine(_response!.RawContent);
                }

                Assert.Equal(statusCode, (int)_response!.StatusCode);
            } else
            {
                AssertWithMessage.Failed("No web response received. Is the scenario missing a step?");
            }
        }

        [Then(@"the element ""([^""]*)"" should have the following markup:")]
        public void ThenTheElementShouldHaveTheFollowingMarkup(string selector, string expectedMarkup)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var actual = Minify(element!);

            var parser = new HtmlParser();
            var document = parser.ParseDocument($@"<div id=""__test__"">{expectedMarkup}</div>");
            var expected = Minify(document!.QuerySelector("#__test__ > *")!);

            Assert.Equal(expected, actual);
        }

        [Then(@"the element ""([^""]*)"" should have the text content ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheTextContent(string selector, string textContent)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(textContent.Trim(), element!.TextContent.Trim());
        }

        [Then(@"the element ""([^""]*)"" should have the tag name ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheTagName(string selector, string expectedTagName)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(expectedTagName, element!.TagName.ToLower());
        }

        [Then(@"the element ""([^""]*)"" should have the class ""([^""]*)""")]
        public void ThenTheElementShouldHaveTheClass(string selector, string expectedClass)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(expectedClass, element!.ClassName);
        }

        [Then(@"the textbox ""([^""]*)"" should have the value ""([^""]*)""")]
        public void ThenTheTextBoxShouldHaveTheValue(string selector, string value)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var input = Assert.IsAssignableFrom<IHtmlInputElement>(element);

            Assert.Equal(value.Trim(), input!.Value.Trim());
        }

        [Then(@"the anchor ""([^""]*)"" should be an internal link to ""([^""]*)""")]
        public void ThenTheAnchorShouldBeAnInternalLinkTo(string selector, string location)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var anchor = Assert.IsAssignableFrom<IHtmlAnchorElement>(element);

            Assert.Equal($"http://localhost{location.Trim()}", anchor.Href.Trim());
        }

        [Then(@"the anchor ""([^""]*)"" should be an external link to ""([^""]*)""")]
        public void ThenTheAnchorShouldBeAnExternalLinkTo(string selector, string location)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            var anchor = Assert.IsAssignableFrom<IHtmlAnchorElement>(element);

            Assert.Equal($"{location.Trim()}", anchor.Href.Trim());
        }

        [Then(@"the page title should be ""([^""]*)""")]
        public void ThenThePageTitleShouldBe(string expected)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var actual = _response!.HtmlContent.Title;

            Assert.Equal(expected, actual);
        }

        [Then(@"page content ""([^""]*)"" should have property ""([^""]*)"" set to ""([^""]*)""")]
        public void ThenPageContentShouldHavePropertySetTo(string id, string propertyPath, string propertyValue)
        {
            GetPageContent(id).SwitchFirst(
                data => {
                    var value = GetPropertyPathValue(propertyPath, data);

                    Assert.Equal(propertyValue, value);
                },
                e => AssertWithMessage.Failed(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        [Then(@"page content ""([^""]*)"" should have property ""([^""]*)"" set to:")]
        public void ThenPageContentShouldHavePropertySetToMultiline(string id, string propertyPath, string propertyValue)
        {
            GetPageContent(id).SwitchFirst(
                data => {
                    string? expectedSerializedPropertyValue = null;
                    JsonHelper.Deserialize<object>(propertyValue).SwitchFirst(
                        v => expectedSerializedPropertyValue = JsonHelper.Serialize(v),
                        e => AssertWithMessage.Failed(e.Description)
                    );

                    var value = GetPropertyPathValue(propertyPath, data);

                    var serializedPropertyValue = JsonHelper.Serialize(value);
                    Assert.Equal(expectedSerializedPropertyValue, serializedPropertyValue);
                },
                e => AssertWithMessage.Failed(@$"Could not find page content with id ""{id}"": {e.Description}")
            );
        }

        private object GetPropertyPathValue(string propertyPath, string data)
        {
            var parts = propertyPath.Split('.');
            var propertyName = parts[0];
            var regex = new Regex(@"(.*)\[(\d+)\]");
            var match = regex.Match(propertyName);
            if (match.Success)
            {
                var arrayProperty = match.Groups[1].Value;
                var arrayIndex = int.Parse(match.Groups[2].Value);

                Dictionary<string, object>? dict = null;
                JsonHelper.Deserialize<Dictionary<string, object>>(data).SwitchFirst(
                    v => dict = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(arrayProperty), $@"Page content object does not contain property ""{arrayProperty}"":\n{data}");
                var arrayValue = dict![arrayProperty];
                object[]? array = null;
                JsonHelper.Deserialize<object[]>(JsonHelper.Serialize(arrayValue)).SwitchFirst(
                    v => array = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                if(array!.Length <= arrayIndex)
                {
                    AssertWithMessage.Failed($@"Array index {arrayIndex} does not exist on array ""{arrayProperty}"":\n{JsonHelper.Serialize(array!)}");
                }
                var value = array![arrayIndex];
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, JsonHelper.Serialize(value));
                }
                return value;
            }
            else
            {
                Dictionary<string, object>? dict = null;
                JsonHelper.Deserialize<Dictionary<string, object>>(data).SwitchFirst(
                    v => dict = v,
                    e => AssertWithMessage.Failed(e.Description)
                );
                AssertWithMessage.True(dict!.ContainsKey(propertyName), $@"Page content object does not contain property ""{propertyName}"":\n{data}");
                var value = dict![propertyName];
                var restOfPath = string.Join(".", parts.Skip(1));
                if (restOfPath.Length > 0)
                {
                    return GetPropertyPathValue(restOfPath, JsonHelper.Serialize(value));
                }
                return value;
            }
        }

        private string Minify(IElement element)
        {
            var minified = element.ToHtml(_minifier);

            return _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
        }

        private async Task<ErrorOr<string>> GetPageContent(string id)
        {
            return await _web.PageContentRepository.Get(id).Then(JsonConvert.SerializeObject);
        }

        private async Task<ErrorOr<Updated>> SetUpPageContent(string id, string data)
        {
            var document = await _web.PageContentRepository.Get(id)
                .Match(v => JsonConvert.SerializeObject(v), _ => $$"""
                {
                    "id": "{{id}}",
                    "contentId": "{{id}}",
                    "published": true,
                    "PageTitle": null,
                    "PageContent": {},
                    "Views": []
                }
            """);

            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            var docDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(document);
            foreach (var d in dataDict)
            {
                docDict[d.Key] = d.Value;
            }

            var template = JsonConvert.DeserializeObject<PageContentTemplate>(JsonConvert.SerializeObject(docDict));

            return await _web.PageContentRepository.Update(template);
        }
    }
}
