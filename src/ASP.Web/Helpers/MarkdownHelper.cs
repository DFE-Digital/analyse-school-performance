using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;

namespace ASP.Web.Helpers
{
    public static class MarkdownHelper
    {
        public static HtmlString ConvertInlineMarkdown(dynamic input)
        {
            // Convert input to string
            string inputString = Convert.ToString(input);

            // Convert inline bold text: **text** or __text__
            inputString = Regex.Replace(inputString, @"\*\*(.+?)\*\*|__(.+?)__", "<strong>$1$2</strong>");

            // Convert inline italic text: *text* or _text_
            inputString = Regex.Replace(inputString, @"\*(.+?)\*|_(.+?)_", "<em>$1$2</em>");

            // Convert inline links: [link text](url)
            inputString = Regex.Replace(inputString, @"\[([^\]]+)\]\(([^)]+)\)", "<a href=\"$2\">$1</a>");


            return new HtmlString(inputString);
        }
    }
}
