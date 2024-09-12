using System.Security.Claims;

namespace ASP.Core.Authorization;

public sealed class Role
{
    public string Code { get; }
    public string Name { get; }
    public bool IsNamed { get; }

    public Role(string code, string name, bool isNamed)
    {
        Code = code;
        Name = name;
        IsNamed = isNamed;
    }

    public bool HasAccessToSearch => AccessToSearch.Contains(this);
    public bool HasAccessToMySchool => AccessToMySchool.Contains(this);
    public bool HasAccessToMySchools => AccessToMySchools.Contains(this);
    public bool HasAccessToMyLaSchools => AccessToMyLaSchools.Contains(this);
    public bool HasAccessToMyMatSchools => AccessToMyMatSchools.Contains(this);
    public bool HasAccessToMyDioceseSchools => AccessToMyDioceseSchools.Contains(this);
    public bool HasAccessToMyLocalAuthority => AccessToMyLocalAuthority.Contains(this);
    public bool HasAccessToAllSchools => AccessToAllSchools.Contains(this);
    public bool HasAccessToAllLocalAuthoritiess => AccessToAllLocalAuthorities.Contains(this);
    public bool HasAccessToEditPages => AccessToEditPages.Contains(this);
    public bool HasAccessToGuidance => AccessToGuidance.Contains(this);
    public bool IsLaUser => AccessToMyLocalAuthority.Contains(this);
    public bool IsSchoolUser => AccessToMySchool.Contains(this);
    public bool IsAny => All.Contains(this);

    public override bool Equals(object? obj)
    {
        return obj is Role other && other.Code.Equals(Code);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public override string? ToString()
    {
        return Name;
    }

    public static bool operator ==(Role? a, Role? b) => a is null && b is null || a is not null && a.Equals(b);
    public static bool operator !=(Role? a, Role? b) => !(a == b);

    public static Role? FromCode(string code) => All.Find(code);

    public static Role? FromName(string name) => All.FindByName(name);

    public static Role? FromClaimsPrincipal(ClaimsPrincipal user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(FromCode)
            .Where(r => r != null)
            .FirstOrDefault();
    }

    public static readonly Role DfeUnnamed = new("RAISE_DfE_Anon", "DfE Unnamed", false);
    public static readonly Role DfeNamed = new("RAISE_DfE_Named", "DfE Named", true);

    public static readonly Role DioceseUnnamed = new("RAISE_Diocese_Anon", "Diocese Unnamed", false);
    public static readonly Role DioceseNamed = new("RAISE_Diocese_Named", "Diocese Named", true);

    public static readonly Role LaUnnamed = new("RAISE_LA_Anon", "LA Unnamed", false);
    public static readonly Role LaNamed = new("RAISE_LA_Named", "LA Named", true);

    public static readonly Role MatUnnamed = new("RAISE_MAT_Anon", "MAT Unnamed", false);
    public static readonly Role MatNamed = new("RAISE_MAT_Named", "MAT Named", true);
    public static readonly Role MatGovernor = new("RAISE_MAT_Governor", "MAT Governor", false);

    public static readonly Role OfstedUnnamed = new("RAISE_Ofsted_Anon", "Ofsted Unnamed", false);

    public static readonly Role SchoolUnnamed = new("RAISE_School_Anon", "School Unnamed", false);
    public static readonly Role SchoolNamed = new("RAISE_School_Named", "School Named", true);
    public static readonly Role SchoolGovernor = new("RAISE_School_Governor", "School Governor", false);

    public static readonly Role SuperUser = new("RAISE_Super_User", "Super Admin", true);

    public static readonly Role TrainingUnnamed = new("RAISE_Training_Anon", "Training Unnamed", false);

    public static RoleCollection Any => All;

    public static readonly RoleCollection All =
    [
        DfeUnnamed,
        DfeNamed,
        DioceseUnnamed,
        DioceseNamed,
        LaUnnamed,
        LaNamed,
        MatUnnamed,
        MatNamed,
        OfstedUnnamed,
        SchoolUnnamed,
        SchoolNamed,
        MatGovernor,
        SchoolGovernor,
        SuperUser,
        TrainingUnnamed
    ];

    public static readonly RoleCollection AccessToSearch =
    [
        DfeUnnamed,
        DfeNamed,
        DioceseUnnamed,
        DioceseNamed,
        LaUnnamed,
        LaNamed,
        MatUnnamed,
        MatNamed,
        OfstedUnnamed,
        MatGovernor,
        SuperUser
    ];

    public static readonly RoleCollection AccessToMySchool =
    [
        SchoolNamed,
        SchoolUnnamed,
        SchoolGovernor
    ];

    public static readonly RoleCollection AccessToMySchools =
    [
        LaUnnamed,
        LaNamed,
        MatUnnamed,
        MatNamed,
        MatGovernor,
        DioceseUnnamed,
        DioceseNamed,
    ];

    public static readonly RoleCollection AccessToMyLaSchools =
    [
        LaUnnamed,
        LaNamed,
    ];

    public static readonly RoleCollection AccessToMyMatSchools =
    [
        MatUnnamed,
        MatNamed,
        MatGovernor,
    ];

    public static readonly RoleCollection AccessToMyDioceseSchools =
    [
        DioceseUnnamed,
        DioceseNamed,
    ];

    public static readonly RoleCollection AccessToMyLocalAuthority =
    [
        LaUnnamed,
        LaNamed
    ];

    public static readonly RoleCollection AccessToAllLocalAuthorities =
    [
        DfeUnnamed,
        DfeNamed,
        OfstedUnnamed,
        SuperUser,
    ];

    public static readonly RoleCollection AccessToAllSchools =
    [
        DfeUnnamed,
        DfeNamed,
        OfstedUnnamed,
        SuperUser,
    ];

    public static readonly RoleCollection AccessToGuidance =
    [
        SchoolNamed,
        SchoolUnnamed,
        MatUnnamed,
        MatNamed,
        MatGovernor,
        DioceseUnnamed,
        DioceseNamed
    ];


    public static readonly RoleCollection AccessToEditPages =
    [
        SuperUser
    ];
}