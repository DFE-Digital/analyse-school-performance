using ASP.Core.Templating;

namespace ASP.Web.Models
{
    [EditModelFor(ViewId = "Heading")]
    public class HeadingEditModel : TemplateComponentEditModel
    {
        public const string HeadingType = nameof(HeadingType);
        public const string Caption = nameof(Caption);
        public const string Text = nameof(Text);
        public const string LinkUrl = nameof(LinkUrl);

        private static readonly Dictionary<string, ViewContentPropertyConfig> _viewContentProperties = new() {
            { HeadingType, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "h2", Preprocess = v => 
                v switch {
                    "h3" => "h3",
                    _ => "h2"
                } } },
            { Caption, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "" } },
            { Text, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "" } },
            { LinkUrl, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "" } }
        };

        public HeadingEditModel()
            : base()
        {
        }

        public HeadingEditModel(TemplateComponent contentTemplate)
            : base(contentTemplate)
        {
        }

        public override Dictionary<string, ViewContentPropertyConfig> ViewContentProperties => _viewContentProperties;
    }
}
