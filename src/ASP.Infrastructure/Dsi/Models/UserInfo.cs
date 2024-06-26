namespace ASP.Infrastructure.Dsi.Models
{
    public class UserInfo
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";

        public IEnumerable<Organisation>? Organisations { get; set; }
    }
}
