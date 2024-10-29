using ASP.Core.Extensions;

namespace Xunit
{
    public partial class Assert
    {
        public static void Equal(string expected, string actual, StringComparison comparisonType)
        {
            Assert.Equal(expected, actual, StringComparer.FromComparison(comparisonType));
        }

        public static void Contains(string key, Dictionary<string, string> dictionary, StringComparison comparisonType)
        {
            Assert.Contains(key, dictionary.Keys, StringComparer.FromComparison(comparisonType));
        }

        public static void Contains(string key, string expectedValue, Dictionary<string, string> dictionary, StringComparison comparisonType)
        {
            Assert.Contains(key, dictionary, comparisonType);
            if (!dictionary.TryGetValue(key, out var value, comparisonType))
            {
                Assert.Fail($@"Dictionary did not contain key ""{key}""");
            }
            else
            {
                Assert.Equal(expectedValue, value, comparisonType);
            }
        }
    }
}
