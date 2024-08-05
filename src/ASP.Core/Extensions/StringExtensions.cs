using ASP.Core.Establishments.Search;

namespace ASP.Core.Extensions;

public static class StringExtensions    
{
    public static SearchType ClassifySearchType(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return SearchType.Invalid;
        }
        
        // Evaluate regex matches before the switch
        var isUrn = Constants.UrnRegex.IsMatch(input);
        var isLaEstab = Constants.LaEstabRegex.IsMatch(input);
        var isLaEstab3Digit = Constants.LaEstab3DigitRegex.IsMatch(input);
        var isLaEstab4Digit = Constants.LaEstab4DigitRegex.IsMatch(input);
        var isLaEstab7Digit = Constants.LaEstab7DigitRegex.IsMatch(input);
        
        // Use a switch statement on the tuple of regex matches
        return (isUrn, isLaEstab, isLaEstab3Digit, isLaEstab4Digit, isLaEstab7Digit) switch
        {
            (true, _, _, _,_) => SearchType.Urn,
            (_, true, _, _,_) => SearchType.LocalAuthEstablishment,
            (_, _, true, _,_) => SearchType.LocalAuthEstablishment3Digit,
            (_, _, _, true,_) => SearchType.LocalAuthEstablishment4Digit,
            (_, _, _, _,true) => SearchType.LocalAuthEstablishment7Digit,
            _ => SearchType.EstablishmentNameOrLocation
        };
    }
    
    public static string ToLaEstabCodeFormat(this string input)
    {
        // Validate that the input exactly matches the 7-digit pattern
        if (string.IsNullOrWhiteSpace(input) || !Constants.LaEstab7DigitRegex.IsMatch(input))
        {
            throw new ArgumentException("Input must be exactly 7 digits long and numeric.");
        }
        // Format the string by inserting a slash after the third digit
        //  3-digit code, a slash, then a 4-digit code
        return $"{input.Substring(0, 3)}/{input.Substring(3)}";
    }
}