using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System.Web;

namespace ASP.Web.Helpers
{
    public static class MarkdownHelper
    {
        // Matches the inner text in e.g. "**text**" or "*text*" but not "** text **" or "* text *"
        private const string ASTERISK_WRAPPED_TEXT = @"[^\s\*]|[^\s\*](?:.|\n)*?[^\s\*]";

        // Matches the inner text in e.g. "__text__" or "_text_" but not "__ text __" or "_ text _"
        private const string UNDERSCORE_WRAPPED_TEXT = @"[^\s_]|[^\s_](?:.|\n)*?[^\s_]";

        private static readonly Regex _markdown = new Regex(string.Join("|", new[] {
            // Matches "*****text*****" but not "***** text *****"
            $@"\*\*\*\*\*(?<BOLD_BOLD_ITALIC_ASTERISK>{ASTERISK_WRAPPED_TEXT})\*\*\*\*\*",

            // Matches "_____text_____" but not "_____ text _____"
            $@"_____(?<BOLD_BOLD_ITALIC_UNDERSCORE>{UNDERSCORE_WRAPPED_TEXT})_____",

            // Matches "****text****" but not "**** text ****"
            $@"\*\*\*\*(?<BOLD_BOLD_ASTERISK>{ASTERISK_WRAPPED_TEXT})\*\*\*\*",

            // Matches "____text____" but not "____ text ____"
            $@"____(?<BOLD_BOLD_UNDERSCORE>{UNDERSCORE_WRAPPED_TEXT})____",

            // Matches "***text***" but not "*** text ***"
            $@"\*\*\*(?<BOLD_ITALIC_ASTERISK>{ASTERISK_WRAPPED_TEXT})\*\*\*",

            // Matches "___text___" but not "___ text ___"
            $@"___(?<BOLD_ITALIC_UNDERSCORE>{UNDERSCORE_WRAPPED_TEXT})___",

            // Matches "**text**" but not "** text **"
            $@"\*\*(?<BOLD_ASTERISK>{ASTERISK_WRAPPED_TEXT})\*\*",

            // Matches "__text__" but not "__ text __"
            $@"__(?<BOLD_UNDERSCORE>{UNDERSCORE_WRAPPED_TEXT})__",

            // Matches "*text*" but not "* text *"
            // We also don't want to match the * in e.g. "** text **" as these are not matched by the regexes above
            // and so will fall through to this one.
            // We're matching the preceding and following characters too so we'll need to add them back in when we 
            // do the replace.
            $@"(^|[^\*])\*(?<ITALIC_ASTERISK>{ASTERISK_WRAPPED_TEXT})\*($|[^\*])",
            
            // Matches "_text_" but not "_ text _"
            // We also don't want to match the _ in e.g. "__ text __" as these are not matched by the regexes above
            // and so will fall through to this one.
            // We're matching the preceding and following characters too so we'll need to add them back in when we 
            // do the replace.
            $@"(^|[^_])_(?<ITALIC_UNDERSCORE>{UNDERSCORE_WRAPPED_TEXT})_($|[^_])",

            // Matches "[text](url){target}"
            @"\[(?<LINK_TARGET>.*?)\]\((.*?)\)\{([^}]*)\}",

            // Matches "[text](url)"
            @"\[(?<LINK>[^\]]+)\]\(([^)]+)\)"
        }), RegexOptions.Compiled | RegexOptions.Multiline);

        public static HtmlString ConvertInlineMarkdown(object input)
        {
            // Convert input to string
            var inputString = input switch
            {
                null => "",
                string s => s,
                _ => Convert.ToString(input) ?? ""
            };

            // Escape HTML
            inputString = HttpUtility.HtmlEncode(inputString);

            // We need to keep performing the replace until the inputString is unchanged.
            // This is because a string like "**_text_**" will match first with the "**" and be replaced with
            // "<strong>_text_</strong>" - but then we've already passed that point in the string and won't pick
            // up the "_text_" part unless we do the replace again.
            while (true)
            {
                var newString = _markdown.Replace(inputString, m =>
                {
                    // Find all the matching groups
                    var groups = m.Groups.Values
                        .Where(g => g.Success && g.Name != "0")
                        .ToArray();

                    if (groups.Length == 0)
                    {
                        return m.Value;
                    }

                    // Each match type has one named group. Some match types have other groups
                    // i.e. ITALIC_ASTERISK, ITALIC_UNDERSCORE and LINK. These groups will have numbers
                    // as names so will appear first in the list.
                    var namedGroup = groups[groups.Length - 1];

                    switch (namedGroup.Name)
                    {
                        case "BOLD_BOLD_ITALIC_ASTERISK":
                        case "BOLD_BOLD_ITALIC_UNDERSCORE":
                            return $"<strong><strong><em>{namedGroup.Value}</em></strong></strong>";

                        case "BOLD_BOLD_ASTERISK":
                        case "BOLD_BOLD_UNDERSCORE":
                            return $"<strong><strong>{namedGroup.Value}</strong></strong>";

                        case "BOLD_ITALIC_ASTERISK":
                        case "BOLD_ITALIC_UNDERSCORE":
                            return $"<strong><em>{namedGroup.Value}</em></strong>";

                        case "BOLD_ASTERISK":
                        case "BOLD_UNDERSCORE":
                            return $"<strong>{namedGroup.Value}</strong>";

                        case "ITALIC_ASTERISK":
                        case "ITALIC_UNDERSCORE":
                            // First and second groups are the preceding and following characters, so we add them
                            // back to the string
                            return $"{groups[0].Value}<em>{namedGroup.Value}</em>{groups[1].Value}";

                        case "LINK":
                            // First group is the URL
                            return $"<a href=\"{groups[0].Value}\" class=\"govuk-link\">{namedGroup.Value}</a>";
                        case "LINK_TARGET":
                            return $"<a href=\"{groups[0].Value}\" class=\"govuk-link\" {groups[1].Value}>{namedGroup.Value}</a>";
                        default:
                            return m.Value;
                    }
                });

                // Each individual transformation above increases the length of the overall string
                // (each replacement string is longer than the matched string) so we can just compare lengths
                // to see if anything's changed
                if (newString.Length == inputString.Length)
                {
                    break;
                }

                inputString = newString;
            }

            return new HtmlString(inputString);
        }
    }
}
