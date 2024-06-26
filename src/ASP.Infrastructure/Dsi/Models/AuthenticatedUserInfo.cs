namespace ASP.Infrastructure.Dsi.Models
{
    public class AuthenticatedUserInfo : UserInfo
    {
        public IEnumerable<UserRole?> Roles { get; set; } = [];

    }
}
