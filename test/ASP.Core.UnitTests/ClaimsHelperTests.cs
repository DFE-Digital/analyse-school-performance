using System.Security.Claims;
using ASP.Core.Helpers;
using Xunit;

namespace ASP.Core.UnitTests;

public class ClaimsHelperTests
{
    [Theory]
    [InlineData(new string[] { "Role1" }, "Role1")]
    [InlineData(new string[] { "", "Role2" }, "Role2")]
    [InlineData(new string[] { null, "Role3" }, "Role3")]
    [InlineData(new string[] { "Role4", "" }, "Role4")]
    [InlineData(new string[] { "Role5", null }, "Role5")]
    [InlineData(new string[] { "", null }, null)]
    [InlineData(new string[] { null, "" }, null)]
    public void GetFirstNonEmptyRoleClaim_ReturnsExpectedRole(string?[] roles, string expectedRole)
    {
        // Arrange
        var claims = roles.Select(r => new Claim(ClaimTypes.Role, r ?? string.Empty)).ToList();
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = ClaimsHelper.GetFirstNonEmptyRoleClaim(user);

        // Assert
        Assert.Equal(expectedRole, result);
    }

    [Fact]
    public void GetFirstNonEmptyRoleClaim_WithNoRoleClaims_ReturnsNull()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "TestUser") };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = ClaimsHelper.GetFirstNonEmptyRoleClaim(user);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetFirstNonEmptyRoleClaim_WithNullUser_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => ClaimsHelper.GetFirstNonEmptyRoleClaim(null));
    }
}