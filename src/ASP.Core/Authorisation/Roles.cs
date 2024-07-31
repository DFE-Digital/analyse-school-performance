namespace ASP.Core.Authorisation;

public static class Roles
{
    public const string DfeUnnamed = "RAISE_DfE_Anon";
    public const string DfeNamed = "RAISE_DfE_Named";

    public const string DioceseUnnamed = "RAISE_Diocese_Anon";
    public const string DioceseNamed = "RAISE_Diocese_Named";

    public const string LaUnnamed = "RAISE_LA_Anon";
    public const string LaNamed = "RAISE_LA_Named";

    public const string MatUnnamed = "RAISE_MAT_Anon";
    public const string MatNamed = "RAISE_MAT_Named";

    public const string OfstedUnnamed = "RAISE_Ofsted_Anon";

    public const string SchoolUnnamed = "RAISE_School_Anon";
    public const string SchoolNamed = "RAISE_School_Named";

    public const string MatGovernor = "RAISE_MAT_Governor";

    public const string SchoolGovernor = "RAISE_School_Governor";

    public const string SuperUser = "RAISE_Super_User";

    public const string TrainingUnnamed = "RAISE_Training_Anon";

    public static readonly string[] All =
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

    public static readonly string[] WithSearch =
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

    public static readonly string[] MySchools =
    [
        LaUnnamed,
        LaNamed,
        MatUnnamed,
        MatNamed,
        MatGovernor,
        DioceseUnnamed,
        DioceseNamed,
    ];
    
    public static readonly string[] AccessToAllLas =
    [
        DfeUnnamed,
        DfeNamed,
        OfstedUnnamed,
        SuperUser,
    ];
}