namespace ASP.Application.UseCases.Downloads
{
    public class Source
    {
        public string RawValue { get; }

        public Source(string value)
        {
            RawValue = Normalize(value) ?? throw new ArgumentException($"Invalid source: {value}", nameof(value));
        }

        private string? Normalize(string? value)
        {
            return value?.Trim() switch
            {
                "KTS" => "KTS",
                "ASP" => "ASP",
                _ => null
            };
        }

        public string ToFriendlyName() => RawValue switch
        {
            "KTS" => "Key to success",
            "ASP" => "Analyse school performance",
            _ => throw new InvalidOperationException($"Unexpected source value: {RawValue}")
        };
    }
}
