using ASP.Core.Authorization;
using System.Security.Claims;
using Xunit;

namespace ASP.Core.UnitTests;

public class RoleTests
{
    [Theory]
    [InlineData(new string?[] { "RAISE_DfE_Anon" }, "RAISE_DfE_Anon")]
    [InlineData(new string?[] { "", "RAISE_DfE_Named" }, "RAISE_DfE_Named")]
    [InlineData(new string?[] { null, "RAISE_Diocese_Anon" }, "RAISE_Diocese_Anon")]
    [InlineData(new string?[] { "RAISE_Diocese_Named", "" }, "RAISE_Diocese_Named")]
    [InlineData(new string?[] { "RAISE_LA_Anon", null }, "RAISE_LA_Anon")]
    [InlineData(new string?[] { "", null }, null)]
    [InlineData(new string?[] { null, "" }, null)]
    public void FromClaimsPrincipal_ReturnsExpectedRole(string?[] codes, string? expectedCode)
    {
        // Arrange
        Role? expectedRole = expectedCode switch {
            null => null,
            string code => new Role(code, code, false)
        };
        
        var claims = codes.Select(r => new Claim(ClaimTypes.Role, r ?? string.Empty)).ToList();
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = Role.FromClaimsPrincipal(user);

        // Assert
        Assert.Equal(expectedRole, result);
    }

    [Fact]
    public void FromClaimsPrincipal_WithNoRoleClaims_ReturnsNull()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "TestUser") };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = Role.FromClaimsPrincipal(user);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromClaimsPrincipal_WithNullUser_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Role.FromClaimsPrincipal(null));
    }
}