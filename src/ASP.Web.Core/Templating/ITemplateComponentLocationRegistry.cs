namespace ASP.Web.Core.Templating
{
    public interface ITemplateComponentLocationRegistry
    {
        IEnumerable<string> GetComponentLocations();
        void RegisterComponentLocation(string path);
    }
}