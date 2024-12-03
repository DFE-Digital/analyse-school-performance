namespace ASP.Web.Shared.Navigation;

public class NavigationItemViewModel
{
    public string Name { get; }
    public string Path { get; }
    public PathString RequestPath { get; }
    public string? Policy { get; }
    public string? TestId { get; }

    public NavigationItemViewModel(string name, string path, PathString requestPath, string? policy = null, string? testId = null)
    {
        Name = name;
        Path = path;
        RequestPath = requestPath;
        Policy = policy;
        TestId = testId;
    }

    public bool IsCurrent => Path == "/"
        ? RequestPath == "/"
        : RequestPath.StartsWithSegments(Path.TrimEnd('/', ' '));
}
