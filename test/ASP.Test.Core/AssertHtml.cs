using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using System.Text.RegularExpressions;
using Xunit;

namespace ASP.AcceptanceTests.StepDefinitions
{
    public static class AssertHtml
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

        public static void Equivalent(IElement expected, IElement actual)
        {
            var minifiedExpected = Minify(expected);
            var minifiedActual = Minify(actual);

            Assert.Equal(minifiedExpected, minifiedActual);
        }

        private static string Minify(IElement element)
        {
            var minified = element.ToHtml(_minifier);

            return _spacesBeforeOpeningTag.Replace(_spacesAfterClosingTag.Replace(minified, ">$2"), "$1<");
        }
    }
}
