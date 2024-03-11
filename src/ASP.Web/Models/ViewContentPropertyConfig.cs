namespace ASP.Web.Models
{
    public class ViewContentPropertyConfig
    {
        public ViewContentPropertyType PropertyType { get; set; } = ViewContentPropertyType.String;
        public string DefaultValue { get; set; } = "";
        public Func<string, string> Preprocess { get; set; } = v => v;
    }
}
