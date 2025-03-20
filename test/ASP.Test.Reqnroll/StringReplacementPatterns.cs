using System.Text.RegularExpressions;

namespace ASP.Test.Reqnroll;

public class StringReplacementPatterns
{
    private static readonly List<(Regex, string)> _stringRegexPattern =
    [
        // Matches: "(number + n)" at the end of a string
        // Example: "Some text (5 + n)" -> "Some text 10" (when n = 5)
        (new Regex(@"^(?<Alpha>[\s\S]*)\((?<number>[0-9]+) \+ (?<replace>n)\)", RegexOptions.Compiled), "{0}"),

        // Matches: " n" at the end of a string
        // Example: "Some text n" -> "Some text 5" (when n = 5)
        (new Regex(@"^(?<Alpha>[\s\S]*) (?<replace>n)$", RegexOptions.Compiled), " {0}"),

        // Matches: " n " in the middle of a string
        // Example: "Some text n more text" -> "Some text 5 more text" (when n = 5)
        (new Regex(@"^(?<Alpha>[\s\S]+) (?<replace>n) (?<Beta>[\s\S]+)$", RegexOptions.Compiled), " {0} "),

        // Matches: " (number + n) " in the middle of a string
        // Example: "Some text (5 + n) more text" -> "Some text 10 more text" (when n = 5)
        (new Regex(@"^(?<Alpha>[\s\S]+) \((?<number>[0-9]+) \+ (?<replace>n)\) (?<beta>[\s\S]+)$", RegexOptions.Compiled),
            " {0} "),

        // Matches: "n " at the beginning of a string
        // Example: "n Some text" -> "5 Some text" (when n = 5)
        (new Regex(@"^(?<replace>n) (?<Alpha>[\s\S]+)$", RegexOptions.Compiled), "{0} "),

        // Matches: "n" as the entire string
        // Example: "n" -> "5" (when n = 5)
        (new Regex(@"^(?<replace>n)$", RegexOptions.Compiled), "{0}") // n
    ];

    /// <summary>
    /// Replaces occurrences of 'n' or '(number + n)' in the input string with the provided value.
    /// </summary>
    /// <param name="value">The input string to process.</param>
    /// <param name="n">The value to replace 'n' with.</param>
    /// <returns>The processed string with replacements made.</returns>
    public static string ReplaceNInPropertyValue(string value, int n)
    {
        foreach (var (regex, replacement) in _stringRegexPattern)
        {
            value = regex.Replace(value, match =>
            {
                var construct = "";
                for (var inc = 1; inc <= match.Groups.Count - 1; inc++)
                {
                    if (match.Groups[inc].Name == "number" && match.Groups[inc + 1].Name == "replace")
                    {
                        if (int.TryParse(match.Groups[inc].Value, out int baseNumber))
                        {
                            construct += (baseNumber + n).ToString();
                        }

                        inc++;
                    }
                    else if (match.Groups[inc].Name == "replace")
                    {
                        construct += string.Format(replacement, n);
                    }
                    else if (match.Groups[inc].Name == "Alpha" || match.Groups[inc].Name == "Beta")
                    {
                        construct += match.Groups[inc].Value;
                    }
                }

                return construct;
            });
        }

        return value;
    }
}