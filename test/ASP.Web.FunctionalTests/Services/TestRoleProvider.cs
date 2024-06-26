using System.Security.Claims;

namespace ASP.Web.AcceptanceTests.Services
{
    public class TestRoleProvider
    {
        private readonly List<Claim> _roles = [];

        public List<Claim> GetRoles()
        {
            return _roles;
        }

        public void SetRoles(IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                _roles.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        public void ClearRoles()
        {
            _roles.Clear();
            _roles.Add(new Claim(ClaimTypes.Role, ""));
        }
    }
}
