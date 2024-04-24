namespace ASP.Web.Core.Templating
{
    public class TemplateComponentLocationRegistry : ITemplateComponentLocationRegistry
    {
        private readonly HashSet<string> _paths = new HashSet<string>();

        public void RegisterComponentLocation(string path)
        {
            _paths.Add(path);
        }

        public IEnumerable<string> GetComponentLocations()
        {
            return _paths;
        }
    }
}