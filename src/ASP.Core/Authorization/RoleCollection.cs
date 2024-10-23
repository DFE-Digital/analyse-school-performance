using System.Collections;

namespace ASP.Core.Authorization;

public class RoleCollection: IEnumerable<Role>
{
    private readonly List<Role> _roles;

    public RoleCollection()
    {
        _roles = [];
    }

    public RoleCollection(IEnumerable<Role> roles)
    {
        _roles = roles.ToList();
    }

    public void Add(Role role) => _roles.Add(role);

    public Role? Find(string code) => _roles.Find(r => r.Code == code);

    public Role? FindByName(string name) => _roles.Find(r => r.Name == name);

    public IEnumerator<Role> GetEnumerator() => _roles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _roles.GetEnumerator();
}