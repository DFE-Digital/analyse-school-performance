using ASP.Web.Areas.School;

namespace ASP.Web.UnitTests;

public class LinkConverterTests
{
    private static string DefaultUrlGenerator(string urn) => $"/school/{urn}/";
    
    [Fact]
    public void ConvertToLinks_WithSingleSchool_ReturnsCorrectLink()
    {
        // Arrange
        var input = "[Test School](100001)";
        var expected = "<a href=\"/school/100001/\">Test School</a>";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToLinks_WithMultipleSchools_ReturnsCorrectLinks()
    {
        // Arrange
        var input = "[School 1](100001) and [School 2](100002)";
        var expected = "<a href=\"/school/100001/\">School 1</a> and <a href=\"/school/100002/\">School 2</a>";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToLinks_WithMySchoolsFlag_ReturnsMySchoolsLinks()
    {
        // Arrange
        string CreateMySchoolsUrl(string urn) => $"/my-schools/{urn}/";
        var input = "[Test School](100001)";
        var expected = "<a href=\"/my-schools/100001/\">Test School</a>";

        // Act
        var result = LinkConverter.ConvertToLinks(input, CreateMySchoolsUrl);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToLinks_WithComplexText_PreservesNonLinkText()
    {
        // Arrange
        var input =
            "Test School 1 was created as the result of a split from [Test School 2](100002), [Test School 3](100003) and [Test School 4](100004) on 1 October 2020.";
        var expected =
            "Test School 1 was created as the result of a split from <a href=\"/school/100002/\">Test School 2</a>, <a href=\"/school/100003/\">Test School 3</a> and <a href=\"/school/100004/\">Test School 4</a> on 1 October 2020.";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToLinks_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var input = "";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ConvertToLinks_WithNoLinks_ReturnsOriginalString()
    {
        // Arrange
        var input = "This is a test string with no links";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("[Test School](12345)", "/school/12345/", "Test School")]
    [InlineData("[Another School](67890)", "/school/67890/", "Another School")]
    public void ConvertToLinks_WithDifferentSchools_ReturnsCorrectLinks(string input, string expectedUrl,
        string schoolName)
    {
        // Arrange
        var expected = $"<a href=\"{expectedUrl}\">{schoolName}</a>";

        // Act
        var result = LinkConverter.ConvertToLinks(input, DefaultUrlGenerator);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ConvertToLinks_WithNull_ReturnsEmptyString()
    {
        // Act
        var result = LinkConverter.ConvertToLinks(null, DefaultUrlGenerator);

        // Assert
        Assert.Equal("", result);
    }
}