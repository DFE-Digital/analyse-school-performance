using System.Text.RegularExpressions;

namespace ASP.Core;

public static class Constants
{
    public const int SearchResultPageSize = 50;
    
    // Matches exactly six digits
    public static readonly Regex UrnRegex = new Regex(@"^\d{6}$", RegexOptions.Compiled); 
    
    // Pattern to match a 3-digit code, a slash, then a 4-digit code
    public static readonly Regex LaEstabRegex = new Regex(@"^\d{3}/\d{4}$", RegexOptions.Compiled);
    
    // Matches exactly three digits
    public static readonly Regex LaEstab3DigitRegex = new Regex(@"^\d{3}$", RegexOptions.Compiled);
    
    // Matches exactly four digits
    public static readonly Regex LaEstab4DigitRegex = new Regex(@"^\d{4}$", RegexOptions.Compiled);
    
    // Matches exactly seven digits
    public static readonly Regex LaEstab7DigitRegex = new Regex(@"^\d{7}$", RegexOptions.Compiled);

    public const string SchoolSearchTermInputValidationMessage =
        "Please enter a search term such as a school name, URN or LAESTAB to start a search";

}