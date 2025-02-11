using System.Text.RegularExpressions;

namespace ASP.Web.Areas.School;

public static class LinkConverter
{
    public static string ConvertToLinks(string input, Func<string, string?> createSchoolUrl)
    {
        // Handle null input
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Pattern to match [School Name](URN)
        var pattern = @"\[(.*?)\]\((\d+)\)";
    
        // Replace matches with HTML links
        var result = Regex.Replace(input, pattern, match =>
        {
            var schoolName = match.Groups[1].Value;
            var urn = match.Groups[2].Value;
            var url = createSchoolUrl(urn) ?? "";
            return $"<a href=\"{url}\">{schoolName}</a>";
        });
    
        return result;
    }
}