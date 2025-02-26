using ASP.Core.Results;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace ASP.Domain.Schools
{
    public record SchoolUrn
    {
        // Matches exactly six digits
        public static readonly Regex UrnRegex = new Regex(@"^\d{6}$", RegexOptions.Compiled);

        public string Value { get; }

        private SchoolUrn(string value)
        {
            Value = value;
        }

        public static Result<SchoolUrn> Parse(string stringValue)
        {
            return UrnRegex.IsMatch(stringValue)
                ? new SchoolUrn(stringValue)
                : Error.Invalid($@"""{stringValue}"" is not a valid school URN.");
        }

        public static bool TryParse(string stringValue, [NotNullWhen(true)] out SchoolUrn? urn)
        {
            if (UrnRegex.IsMatch(stringValue))
            {
                urn = new SchoolUrn(stringValue);
                return true;
            }

            urn = null;
            return false;
        }
    }
}
