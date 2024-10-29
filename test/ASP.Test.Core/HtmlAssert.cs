using AngleSharp.Dom;
using ASP.Test.Core;

namespace Xunit
{
    public partial class Assert
    {
        public static void HtmlEqual(IElement expected, IElement actual, Action<string>? output = null)
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

            Equal(minifiedExpected, minifiedActual);
        }

        public static void HtmlEqual(string expected, string actual, Action<string>? output = null)
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

            Equal(minifiedExpected, minifiedActual);
        }

        public static void HtmlEqual(string expected, IElement actual, Action<string>? output = null)
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

            Equal(minifiedExpected, minifiedActual);
        }
    }
}
