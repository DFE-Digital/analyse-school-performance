namespace ASP.Infrastructure.Dsi.Models
{
    public class UserAccess
    {
        public Guid? UserId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? OrganisationId { get; set; }
        public IEnumerable<UserRole>? Roles { get; set; }
        public IEnumerable<KeyValue>? Identifiers { get; set; }
    }
}
