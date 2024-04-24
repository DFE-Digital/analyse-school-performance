namespace ASP.Web.Core.Templating
{
    public class ViewContentPropertyConfig
    {
        public ViewContentPropertyType PropertyType { get; set; } = ViewContentPropertyType.String;
        public object? DefaultValue { get; set; } = "";
        public Func<string, string> Preprocess { get; set; } = v => v;
    }
}
