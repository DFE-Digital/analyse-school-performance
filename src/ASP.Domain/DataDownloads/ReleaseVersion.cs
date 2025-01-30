using ASP.Core.Results;
using System.Text.RegularExpressions;

namespace ASP.Domain.DataDownloads
{
    public class ReleaseVersion
    {
        private static readonly Dictionary<string, ReleaseVersion> _allowedVersions = new() {
            ["provisional"] = new ReleaseVersion("provisional", "Provisional", 1),
            ["provisional_with_cla"] = new ReleaseVersion("provisional_with_cla", "Provisional, with CLA", 1),
            ["provisional_without_cla"] = new ReleaseVersion("provisional_without_cla", "Provisional, without CLA", 1),
            ["revised"] = new ReleaseVersion("revised", "Revised", 2),
            ["revised_with_cla"] = new ReleaseVersion("revised_with_cla", "Revised, with CLA", 2),
            ["revised_without_cla"] = new ReleaseVersion("revised_without_cla", "Revised, without CLA", 2),
            ["final"] = new ReleaseVersion("final", "Final", 3),
            ["final_with_cla"] = new ReleaseVersion("final_with_cla", "Final, with CLA", 3),
            ["final_without_cla"] = new ReleaseVersion("final_without_cla", "Final, without CLA", 3)
        };

        public string RawValue { get; }
        public string FriendlyName { get; }
        public int Priority { get; }

        private ReleaseVersion(string rawValue, string friendlyName, int priority)
        {
            RawValue = rawValue;
            FriendlyName = friendlyName;
            Priority = priority;
        }

        public string ToDashedFormat()
        {
            if (string.IsNullOrWhiteSpace(RawValue))
            {
                throw new ArgumentException("Version cannot be null or whitespace.", nameof(RawValue));
            }

            return Regex.Replace(RawValue, @"[\s_,]+", "-").ToLowerInvariant();
        }

        public static Result<ReleaseVersion> Validate(string rawValue)
        {
            if (_allowedVersions.ContainsKey(rawValue))
            {
                return _allowedVersions[rawValue];
            }

            return Error.Invalid(@$"File path part ""{rawValue}"" is not a valid release version.");
        }
    }
}
