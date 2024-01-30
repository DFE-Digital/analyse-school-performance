using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Net;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Infrastructure;

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

        AspWebResponse? _response = null;

        private readonly ISpecFlowOutputHelper _outputHelper;

        public AspWebStepDefinitions(ISpecFlowOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _web = new AspWeb();
        }

        [When(@"I navigate to ((?:/.*)+)")]
        public async Task WhenINavigateTo(string path)
        {
            _response = await _web.GetAsync(path);
        }

        [Then(@"I should get a (.*) response")]
        public void ThenIShouldGetAResponse(int statusCode)
        {
            Assert.NotNull(_response);
            Assert.Equal(statusCode, (int)_response!.StatusCode);
        }

        [Then(@"the HTML element with selector ""(.*)"" should have the following markup:")]
        public void ThenTheHTMLElementWithIdShouldHaveTheFollowingMarkup(string selector, string expectedMarkup)
        {
            Assert.NotNull(_response);

            var element = _response.HtmlContent.QuerySelector(selector);
            Assert.NotNull(element);
            var actual = Minify(element);

            var parser = new HtmlParser();
            var document = parser.ParseDocument($@"<div id=""__test__"">{expectedMarkup}</div>");
            var expected = Minify(document.QuerySelector("#__test__ > *"));

            Assert.Equal(expected, actual);
        }

        [Then(@"the HTML element with selector ""(.*)"" should have the text content ""(.*)""")]
        public void ThenTheHTMLElementWithSelectorShouldHaveTheTextContent(string selector, string textContent)
        {
            Assert.NotNull(_response);

            var element = _response.HtmlContent.QuerySelector(selector);
            
            Assert.Equal(textContent.Trim(), element.TextContent.Trim());
        }

        [Then(@"the page title should be ""(.*)""")]
        public void ThenThePageTitleShouldBe(string expected)
        {
            Assert.NotNull(_response);

            var actual = _response.HtmlContent.Title;

            Assert.Equal(expected, actual);
        }

        private string Minify(IElement element)
        {
            var minified = element.ToHtml(_minifier);

            return _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
        }
    }
}
