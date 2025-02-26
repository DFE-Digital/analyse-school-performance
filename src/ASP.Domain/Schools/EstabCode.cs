using ASP.Core.Results;

namespace ASP.Domain.Schools
{
    public record EstabCode
    {
        public string Value { get; }

        private EstabCode(string value)
        {
            Value = value;
        }

        public static Result<EstabCode> Parse(string stringValue)
        {
            return new EstabCode(stringValue);
        }
    }
}
