using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Web;

namespace ASP.Web.Helpers
{
    public static class MarkdownHelper
    {
        public static HtmlString ConvertInlineMarkdown(dynamic input)
        {
            string inputString = string.Empty;
                
            // Convert input to string
            if(input is not null)
                inputString = Convert.ToString(input);

            // Escape HTML
            inputString = HttpUtility.HtmlEncode(inputString);

            // Convert inline bold and italic text: ***text*** or ___text___ or **_text_** or __*text*__
            inputString = Regex.Replace(inputString, @"\*\*\*(.+?)\*\*\*|___(.+?)___|\*\*_(.+?)_\*\*|__\*(.+?)\*__", "<strong><em>$1$2$3$4</em></strong>");

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
