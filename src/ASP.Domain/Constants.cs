using System.Text.RegularExpressions;

namespace ASP.Domain;

public static class Constants
{
    // Matches a 3-digit LA code or a 6-digit URN
    public static readonly Regex ScopeIdentifierRegex = new(@"^\d{3}$|^\d{6}$", RegexOptions.Compiled);
    
    // Pattern to match a 3-digit code, a slash, then a 4-digit code
    public static readonly Regex LaEstabRegex = new Regex(@"^\d{3}/\d{4}$", RegexOptions.Compiled);
    
    // Matches exactly three digits
    public static readonly Regex LaEstab3DigitRegex = new Regex(@"^\d{3}$", RegexOptions.Compiled);
    
    // Matches exactly four digits
    public static readonly Regex LaEstab4DigitRegex = new Regex(@"^\d{4}$", RegexOptions.Compiled);
    
    // Matches exactly seven digits
    public static readonly Regex LaEstab7DigitRegex = new Regex(@"^\d{7}$", RegexOptions.Compiled);

    //Matches download id from UI
    public static readonly Regex DownloadIdRegex = new(@"(?<ConfigId>.+)-(?<Identifier>\d{3}|\d{6})-(?<Year>\d{4})(?:-(?<Version>.+))?$", RegexOptions.Compiled);
}