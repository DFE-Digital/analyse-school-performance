using System.Text.RegularExpressions;

namespace ASP.Core;

public static class Constants
{
    public const int SearchResultPageSize = 50;
    
    public const int SearchResultMaxSuggestions = 10;
    
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

    //Matches download id from UI
    public static readonly Regex DownloadIdRegex = new(@"(?<ConfigId>.+)-(?<Identifier>\d{3}|\d{6})-(?<Year>\d{4})(?:-(?<Version>.+))?$", RegexOptions.Compiled);

    public const string SchoolSearchTermInputValidationMessage =
        "Please enter a school name, address, URN (Unique Reference Number) or LAESTAB (Local Authority Establishment Number)";

    public const string SchoolSearchFormSearchTermInputLabel =
        "Enter school name, address, URN (Unique Reference Number) or\n LAESTAB (Local Authority Establishment Number)";
    
    public const string LaSearchTermInputValidationMessage =
        "Please enter a local authority name or code";

    public const string LaSearchFormSearchTermInputLabel =
        "Enter local authority name or code";

    public const string AcademicYearToDownldValidationErrorMessage = "Please choose an academic year to download";
    
    public const string DataFilesAvialableForDownlodValidationErrorMessage = "Please choose one or more data files to download";

    public const string ModelErrorKeySelectedYear = "selectedYear";
    
    public const string ModelErrorKeySelectedFiles = "selectedFiles";
    public const string AlpineComponentSchoolSearchSuggestions = "schoolSearchSuggestions";
    
    public const string AlpineComponentLaSearchSuggestions = "laSearchSuggestions";

}