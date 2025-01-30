using ASP.Domain.Templating;
using ASP.Web.Core.Templating;

namespace ASP.Web.Components.Paragraph
{
    [EditModelFor(ViewId = "Paragraph")]
    public class ParagraphEditModel : TemplateComponentEditModel
    {
        public const string IsLarge = nameof(IsLarge);
        public const string Text = nameof(Text);

        private static readonly Dictionary<string, ViewContentPropertyConfig> _viewContentProperties = new() {
            { IsLarge, new() { PropertyType = ViewContentPropertyType.Bool, DefaultValue = false, } },
            { Text, new() { PropertyType = ViewContentPropertyType.String, DefaultValue = "" } },
        };

        public ParagraphEditModel()
            : base()
        {
        }

        public ParagraphEditModel(TemplateComponent contentTemplate, ITemplateComponentEditModelFactory editModelFactory)
            : base(contentTemplate, editModelFactory)
        {
        }

        public override Dictionary<string, ViewContentPropertyConfig> ViewContentProperties => _viewContentProperties;
    }
}
