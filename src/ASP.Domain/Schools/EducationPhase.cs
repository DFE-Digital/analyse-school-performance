namespace ASP.Domain.Schools;

public class EducationPhase
{
    public bool? IsPrimary { get; }
    public bool? IsSecondary { get; }
    public bool? IsPost16 { get; }

    public EducationPhase(bool? isPrimary, bool? isSecondary, bool? isPost16)
    {
        IsPrimary = isPrimary;
        IsSecondary = isSecondary;
        IsPost16 = isPost16;
    }

    public override string? ToString()
    {
        return (IsPrimary, IsSecondary, IsPost16) switch {
            (true, _, _) => "Primary",
            (_, true, _) => "Secondary",
            (_, _, true) => "16 to 18",
            _ => null
        };
    }
}