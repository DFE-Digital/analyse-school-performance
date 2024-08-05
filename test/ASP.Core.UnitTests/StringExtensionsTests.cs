using ASP.Core.Establishments.Search;
using ASP.Core.Extensions;
using Xunit;

namespace ASP.Core.UnitTests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", SearchType.Invalid)]
    [InlineData(null, SearchType.Invalid)]
    [InlineData("123456", SearchType.Urn)]
    [InlineData("894/2200", SearchType.LocalAuthEstablishment)]
    [InlineData("894", SearchType.LocalAuthEstablishment3Digit)]
    [InlineData("2200", SearchType.LocalAuthEstablishment4Digit)]
    [InlineData("SomeOtherInput", SearchType.EstablishmentNameOrLocation)]
    public void ClassifySearchType_Test(string input, SearchType expected)
    {
        SearchType result = input.ClassifySearchType();
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("1234567", "123/4567")]
    [InlineData("9999999", "999/9999")]
    [InlineData("0000000", "000/0000")]  // Edge case: All zeros
    public void ToLaEstabCodeFormat_ValidInput_ShouldFormatCorrectly(string input, string expected)
    {
        // Act
        string result = input.ToLaEstabCodeFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("123456")]  // Less than 7 digits
    [InlineData("12345678")]  // More than 7 digits
    [InlineData("abcd123")]  // Contains non-digit characters
    [InlineData("1234abc")]  // Mixed digits and characters
    public void ToLaEstabCodeFormat_InvalidInput_ShouldThrowArgumentException(string input)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => input.ToLaEstabCodeFormat());
        Assert.Equal("Input must be exactly 7 digits long and numeric.", exception.Message);
    }
}