using AngleSharp.Dom;
using Xunit;

namespace ASP.Test.Core
{
    public partial class AssertHtml
    {
        public static void Equal(IElement expected, IElement actual, Action<string>? output = null)
        {
            var minifiedExpected = Html.Minify(expected);
            if(output != null)
            {
                output($"Expected: {minifiedExpected}");
            }
            var minifiedActual = Html.Minify(actual);
            if (output != null)
            {
                output($"Actual: {minifiedActual}");
            }

            Assert.Equal(minifiedExpected, minifiedActual);
        }

        public static void Equal(string expected, string actual, Action<string>? output = null)
        {
            var minifiedExpected = Html.Minify(expected);
            if (output != null)
            {
                output($"Expected: {minifiedExpected}");
            }
            var minifiedActual = Html.Minify(actual);
            if (output != null)
            {
                output($"Actual: {minifiedActual}");
            }

            Assert.Equal(minifiedExpected, minifiedActual);
        }

        public static void Equal(string expected, IElement actual, Action<string>? output = null)
        {
            var minifiedExpected = Html.Minify(expected);
            if (output != null)
            {
                output($"Expected: {minifiedExpected}");
            }
            var minifiedActual = Html.Minify(actual);
            if (output != null)
            {
                output($"Actual: {minifiedActual}");
            }

            Assert.Equal(minifiedExpected, minifiedActual);
        }
    }
}
