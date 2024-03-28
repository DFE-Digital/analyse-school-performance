using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Parser;
using System.Text.RegularExpressions;

namespace ASP.Test.Core
{
    public static class Html
    {
        private const string TEST_WRAPPER_TAG = "asp-test";
        private static readonly Regex _spacesAfterClosingTag = new Regex(@">([\s\t\n]*)([^\s\t\n])", RegexOptions.Compiled);
        private static readonly Regex _spacesBeforeOpeningTag = new Regex(@"([^\s\t\n])([\s\t\n]*)<", RegexOptions.Compiled);
        private static readonly Regex _testWrapper = new Regex(@$"(<{TEST_WRAPPER_TAG}>)(.*)(</{TEST_WRAPPER_TAG}>)", RegexOptions.Compiled);
        private static readonly MinifyMarkupFormatter _minifier = new MinifyMarkupFormatter {
            ShouldKeepStandardElements = false,
            ShouldKeepAttributeQuotes = true,
            ShouldKeepEmptyAttributes = true,
            ShouldKeepImpliedEndTag = true,
            ShouldKeepComments = false
        };

        public static string Minify(IElement element)
        {
            var minified = element.ToHtml(_minifier);

            return _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
        }

        public static string Minify(string html)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument($@"<{TEST_WRAPPER_TAG}>{html}</{TEST_WRAPPER_TAG}>");
            var element = document!.QuerySelector(TEST_WRAPPER_TAG)!;
            var minified = element.ToHtml(_minifier);

            var stripped = _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
            return _testWrapper.Replace(stripped, "$2");
        }
    }
}
