namespace ASP.Web.Shared
{
    public class NavigationItemViewModel
    {
        public string Id { get; }
        public string Name { get; }
        public string Path { get; }
        public PathString RequestPath { get; }
        public string? Policy { get; }

        public NavigationItemViewModel(string id, string name, string path, PathString requestPath)
        {
            Id = id;
            Name = name;
            Path = path;
            RequestPath = requestPath;
        }

        public NavigationItemViewModel(string id, string name, string path, PathString requestPath, string? policy)
        {
            Id = id;
            Name = name;
            Path = path;
            RequestPath = requestPath;
            Policy = policy;
        }

        public bool IsCurrent => Path == "/" 
            ? RequestPath == "/" 
            : RequestPath.StartsWithSegments(Path.TrimEnd('/', ' '));
    }
}
