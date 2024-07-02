namespace ASP.Core.Utilities;

public static class EducationPhase
{
    public static string? GetPhaseOfEducation(bool? isPrimary, bool? isSecondary, bool? isPost16)
    {
        return (isPrimary, isSecondary, isPost16) switch
        {
            (true, _, _) => "Primary",
            (_, true, _) => "Secondary",
            (_, _, true) => "16 to 18",
            _ => null
        };
    }
}