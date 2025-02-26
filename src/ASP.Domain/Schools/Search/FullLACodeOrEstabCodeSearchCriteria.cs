using ASP.Core.Results;
using System.Text.RegularExpressions;

namespace ASP.Domain.Schools.Search
{
    public record FullLACodeOrEstabCodeSearchCriteria : ISearchCriteria
    {
        public static readonly Regex LAEstabRegex = new Regex(@"^(?<LACode>\d{3})?/?(?<EstabCode>\d{4})?$", RegexOptions.Compiled);

        private string LACode { get; }
        private string EstabCode { get; }
        public string RawValue { get; }

        public string SearchTerm
        {
            get
            {
                return $"{LACode}/{EstabCode}";
            }
        }

        private FullLACodeOrEstabCodeSearchCriteria(string laCode, string estabCode, string rawValue)
        {
            LACode = laCode;
            EstabCode = estabCode;
            RawValue = rawValue;
        }

        public static Result<FullLACodeOrEstabCodeSearchCriteria> Parse(string stringValue)
        {
            var match = LAEstabRegex.Match(stringValue);
            if (match.Success)
            {
                return new FullLACodeOrEstabCodeSearchCriteria(match.Groups["LACode"].Value, match.Groups["EstabCode"].Value, stringValue);
            }

            return Error.Invalid($@"""{stringValue}"" is not a valid LAEstab code.");
        }
    }
}
