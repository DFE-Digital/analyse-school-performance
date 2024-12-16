namespace ASP.Application.UseCases.Downloads
{
    public class DatasetType
    {
        public string RawValue { get; }

        public DatasetType(string value)
        {
            RawValue = Normalize(value) ?? throw new ArgumentException($"Invalid dataset type: {value}", nameof(value));
        }

        private string? Normalize(string? value)
        {
            return value?.Trim() switch
            {
                "Absence" => "Absence",
                "Exclusions" => "Exclusions",
                "KeyStage2" => "KeyStage2",
                "KeyStage4" => "KeyStage4",
                "Post16" => "Post16",
                "QLA" => "QLA",
                "SchoolCharacteristics" => "SchoolCharacteristics",
                "MTC" => "MTC",
                "Phonics" => "Phonics",
                _ => null
            };
        }

        public string ToFriendlyName() => RawValue switch
        {
            "Absence" => "Absence",
            "Exclusions" => "Exclusions",
            "KeyStage2" => "Key stage 2 (KS2)",
            "KeyStage4" => "Key stage 4 (KS4)",
            "Post16" => "16-18",
            "QLA" => "QLA (year 6 only)",
            "SchoolCharacteristics" => "School characteristics",
            "MTC" => "Multiplication table check (MTC)",
            "Phonics" => "Phonics",
            _ => throw new InvalidOperationException($"Unexpected dataset type value: {RawValue}")
        };
    }
}
