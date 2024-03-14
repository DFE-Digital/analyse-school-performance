using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Web;

namespace ASP.Web.Helpers
{
    public static class MarkdownHelper
    {
        // Matches "**text**" or "__text__" but not "** text **" or "__ text __"
        private static readonly Regex _bold = new Regex(@"\*\*([^\s].+[^\s])\*\*|__([^\s].+[^\s])__", RegexOptions.Compiled);

        // Matches "*text*" or "_text_" but not "* text *" or "_ text _"
        // We also don't want to match the * or _ in "** text **" and "__ text __" as these are not matched by the bold regex above
        // and so will fall through to this one.
        private static readonly Regex _italic = new Regex(@"(^|[^\*])\*([^\s\*].+[^\s\*])\*($|[^\*])|(^|[^_])_([^\s_].+[^\s_])_($|[^_])", RegexOptions.Compiled);

        // Matches "[text](url)"
        private static readonly Regex _link = new Regex(@"\[([^\]]+)\]\(([^)]+)\)", RegexOptions.Compiled);

        public static HtmlString ConvertInlineMarkdown(dynamic input)
        {
            string inputString = string.Empty;
                
            // Convert input to string
            if(input is not null)
                inputString = Convert.ToString(input);

            // Escape HTML
            inputString = HttpUtility.HtmlEncode(inputString);

            while (_bold.IsMatch(inputString))
            {
                inputString = _bold.Replace(inputString, "<strong>$1$2</strong>");
            }

            while (_italic.IsMatch(inputString))
            {
                inputString = _italic.Replace(inputString, "$1$4<em>$2$5</em>$3$6");
            }

            inputString = _link.Replace(inputString, "<a href=\"$2\" class=\"govuk-link\">$1</a>");

            return new HtmlString(inputString);
        }
    }
}
