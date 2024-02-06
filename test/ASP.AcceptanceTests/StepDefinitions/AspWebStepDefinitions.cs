using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using ASP.Test.Core;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public sealed class AspWebStepDefinitions
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
        private readonly MemoryStore _store;

        AspWebResponse? _response = null;

        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(ISpecFlowOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _store = new MemoryStore();
            _web = new AspWeb(_store);
        }

        [When(@"I navigate to ((?:/.*)+)")]
        public async Task WhenINavigateTo(string path)
        {
            _response = await _web.GetAsync(path);
        }

        [Then(@"I should get a (.*) response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the scenario missing a step?");

            if (_response!.StatusCode != HttpStatusCode.OK)
            {
                _outputHelper.WriteLine(_response!.RawContent);
            }

            Assert.Equal(statusCode, (int)_response!.StatusCode);
        }

        [Then(@"the HTML element with selector ""(.*)"" should have the following markup:")]
        public void ThenTheHTMLElementWithIdShouldHaveTheFollowingMarkup(string selector, string expectedMarkup)
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

        [Then(@"the HTML element with selector ""(.*)"" should have the text content ""(.*)""")]
        public void ThenTheHTMLElementWithSelectorShouldHaveTheTextContent(string selector, string textContent)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var element = _response!.HtmlContent.QuerySelector(selector);
            AssertWithMessage.NotNull(element, @$"Could not find an element with the selector ""{selector}"".");

            Assert.Equal(textContent.Trim(), element!.TextContent.Trim());
        }

        [Then(@"the page title should be ""(.*)""")]
        public void ThenThePageTitleShouldBe(string expected)
        {
            AssertWithMessage.NotNull(_response, "No web response received. Is the test missing an action?");

            var actual = _response!.HtmlContent.Title;

            Assert.Equal(expected, actual);
        }

        [Given(@"page content ""([^""]*)"" exists:")]
        public void GivenPageContentExists(string id, string data)
        {
            SetUpPageContent(id, data);
        }

        private string Minify(IElement element)
        {
            var minified = element.ToHtml(_minifier);

            return _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
        }

        private static class AssertWithMessage
        {
            public static void NotNull(object? @object, string message)
            {
                try
                {
                    Assert.NotNull(@object);
                }
                catch(XunitException)
                {
                    throw new XunitException(message);
                }
            }
        }

        private void SetUpPageContent(string id, string data)
        {
            var document = Retrieve("content", id, id)
                .Match(v => v, _ => $$"""
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

            Store("content", id, id, JsonConvert.SerializeObject(docDict));
        }

        private void Store(string containerKey, string id, string partitionKeyValue, string document)
        {
            _store.Set(containerKey, id, partitionKeyValue, document);
        }

        private ErrorOr<string> Retrieve(string containerKey, string id, string partitionKeyValue)
        {
            return _store.Get(containerKey, id, partitionKeyValue);
        }
    }
}
