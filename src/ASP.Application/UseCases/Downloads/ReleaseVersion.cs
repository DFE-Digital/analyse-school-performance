using System.Text.RegularExpressions;

namespace ASP.Application.UseCases.Downloads
{
    public class ReleaseVersion
    {
        public string? RawValue { get; }

        public ReleaseVersion(string? rawValue)
        {
            RawValue = rawValue;
        }

        public string? ToFriendlyName()
        {
            return RawValue switch
            {
                "provisional" => "Provisional",
                "provisional_with_cla" => "Provisional, with CLA",
                "provisional_without_cla" => "Provisional, without CLA",
                "revised" => "Revised",
                "revised_with_cla" => "Revised, with CLA",
                "revised_without_cla" => "Revised, without CLA",
                "final" => "Final",
                "final_with_cla" => "Final, with CLA",
                "final_without_cla" => "Final, without CLA",
                _ => null
            };
        }

        public string ToDashedFormat()
        {
            if (string.IsNullOrWhiteSpace(RawValue))
            {
                throw new ArgumentException("Version cannot be null or whitespace.", nameof(RawValue));
            }

            return Regex.Replace(RawValue, @"[\s_,]+", "-").ToLowerInvariant();
        }

        public int GetPriority()
        {
            return RawValue?.ToLower() switch
            {
                "final" => 3,
                "final_with_cla" => 3,
                "final_without_cla" => 3,
                "revised" => 2,
                "revised_with_cla" => 2,
                "revised_without_cla" => 2,
                "provisional" => 1,
                "provisional_with_cla" => 1,
                "provisional_without_cla" => 1,
                _ => 0 // Lowest priority if version is unknown or null
            };
        }

        public string RemoveVersionSuffix(string id)
        {
            int lastDashIndex = id.LastIndexOf('-');
            return lastDashIndex >= 0 ? id.Substring(0, lastDashIndex) : id;
        }
    }
}
