using ASP.Core.Results;
using System.Text.RegularExpressions;

namespace ASP.Domain.Schools
{
    public record LAEstabCode
    {
        // Pattern to match a 3-digit code, a slash, then a 4-digit code
        public static readonly Regex LaEstabRegex = new Regex(@"^\d{3}/\d{4}$", RegexOptions.Compiled);

        // Matches exactly three digits
        public static readonly Regex LaEstab3DigitRegex = new Regex(@"^\d{3}$", RegexOptions.Compiled);

        // Matches exactly four digits
        public static readonly Regex LaEstab4DigitRegex = new Regex(@"^\d{4}$", RegexOptions.Compiled);

        // Matches exactly seven digits
        public static readonly Regex LaEstab7DigitRegex = new Regex(@"^\d{7}$", RegexOptions.Compiled);

        public string Value { get; }

        private LAEstabCode(string value)
        {
            Value = value;
        }

        public static Result<LAEstabCode> Parse(string? stringValue)
        {
            return stringValue != null && LaEstabRegex.IsMatch(stringValue)
                ? Result.Success(new LAEstabCode(stringValue))
                : Result.Invalid<LAEstabCode>($@"""{stringValue}"" is not a valid LAESTAB code.");
        }
    }
}
