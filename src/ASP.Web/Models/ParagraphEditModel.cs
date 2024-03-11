using ASP.Core.PageContent;

namespace ASP.Web.Models
{
    [EditModelFor(ViewId = "Paragraph")]
    public class ParagraphEditModel : TemplateComponentEditModel
    {
        public const string Size = nameof(Size);
        public const string Text = nameof(Text);

        private static readonly Dictionary<string, ViewContentPropertyConfig> _viewContentProperties = new() {
            { Size, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "h2", Preprocess = v => 
                v switch {
                    "l" => "l",
                    "s" => "s",
                    "xs" => "xs",
                    _ => "m",
                } } },
            { Text, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "" } },
        };

        public ParagraphEditModel()
            : base()
        {
        }

        public ParagraphEditModel(PageContentTemplateView contentTemplate)
            : base(contentTemplate)
        {
        }

        public override Dictionary<string, ViewContentPropertyConfig> ViewContentProperties => _viewContentProperties;
    }
}
