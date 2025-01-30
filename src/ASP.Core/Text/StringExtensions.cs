namespace ASP.Core.Text;

public static class StringExtensions
{
    public static string ReplacePrefix(this string input, string oldPrefix, string newPrefix)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(oldPrefix);
        ArgumentNullException.ThrowIfNull(newPrefix);

        return input.Replace(oldPrefix, newPrefix);
    }
}