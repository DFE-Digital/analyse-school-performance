using System.Diagnostics.CodeAnalysis;

namespace ASP.Core.Extensions;

public static class DictionaryExtensions
{
    public static bool TryGetValue<TValue>(this IDictionary<string, TValue> dictionary, string key, [MaybeNullWhen(false)] out TValue value, StringComparison comparisonType)
    {
        value = default;
        var matchingKeyValuePair = dictionary.FirstOrDefault(kvp => kvp.Key.Equals(key, comparisonType));
        
        if (default(KeyValuePair<string, TValue>).Equals(matchingKeyValuePair))
        {
            return false;
        }

        value = matchingKeyValuePair.Value;
        return true;
    }
}
