using ASP.Core.Results;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace ASP.Domain.Schools
{
    public record LACode
    {
        // Matches exactly three digits
        public static readonly Regex LACodeRegex = new Regex(@"^\d{3}$", RegexOptions.Compiled);

        public string Value { get; }

        private LACode(string value)
        {
            Value = value;
        }

        public static Result<LACode> Parse(string stringValue)
        {
            return LACodeRegex.IsMatch(stringValue)
                ? new LACode(stringValue)
                : Error.Invalid($@"""{stringValue}"" is not a valid LA code.");
        }

        public static bool TryParse(string stringValue, [NotNullWhen(true)] out LACode? urn)
        {
            if (LACodeRegex.IsMatch(stringValue))
            {
                urn = new LACode(stringValue);
                return true;
            }

            urn = null;
            return false;
        }
    }
}
