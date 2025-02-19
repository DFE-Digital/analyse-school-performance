using ASP.Api.Client.ContentTemplates;
using ASP.Core.Results;
using ASP.Core.Text;
using Newtonsoft.Json.Linq;

namespace ASP.Web.Core.Templating
{
    public abstract class TemplateComponentEditModel
    {
        private static readonly Dictionary<string, ViewContentPropertyConfig> _viewContentProperties
            = new Dictionary<string, ViewContentPropertyConfig>();

        public TemplateComponentEditModel()
        {
            InitializeTypes();
        }

        public TemplateComponentEditModel(TemplateComponent contentTemplate, ITemplateComponentEditModelFactory editModelFactory)
        {

            ViewId = contentTemplate.ViewId;

            IDictionary<string, object> viewContent = contentTemplate.ViewContent switch
            {
                JObject o => o.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>(),
                IDictionary<string, object> d => d,
                _ => new Dictionary<string, object>()
            };
            ViewContent = viewContent.ToDictionary(c => c.Key, c =>
                c.Value switch
                {
                    null => "",
                    string s => s,
                    bool b => JsonHelper.Serialize(b),
                    double d => d.ToString(),
                    long l => l.ToString(),
                    JArray a => JsonHelper.SerializeIndented(a),
                    JObject o => JsonHelper.SerializeIndented(o),
                    _ => c.Value.ToString() ?? ""
                });

            IList<TemplateComponent> childViews = contentTemplate.ChildViews ?? new List<TemplateComponent>();
            ChildViews = childViews.Select(editModelFactory.CreateTemplateComponentEditModel).ToList();

            InitializeTypes();

            foreach (var (key, value) in viewContent)
            {
                if (!Types.ContainsKey(key))
                {
                    Types[key] = value switch
                    {
                        null => ViewContentPropertyType.Json,
                        string s => ViewContentPropertyType.String,
                        bool b => ViewContentPropertyType.Bool,
                        double d => ViewContentPropertyType.Double,
                        long l => ViewContentPropertyType.Long,
                        _ => ViewContentPropertyType.Json,
                    };
                }
            }

            InitializeViewContent();
        }

        public string ViewId { get; set; } = "";

        private Dictionary<string, string> _viewContent = new Dictionary<string, string>();
        public Dictionary<string, string> ViewContent
        {
            get
            {
                return _viewContent;
            }
            set
            {
                _viewContent = value;
                InitializeViewContent();
            }
        }

        public List<TemplateComponentEditModel> ChildViews { get; set; } = new();
        public Dictionary<string, ViewContentPropertyType> Types { get; set; } = new();
        public virtual Dictionary<string, ViewContentPropertyConfig> ViewContentProperties => _viewContentProperties;

        public Result<TemplateComponent> ToTemplate()
        {
            var serialized = JsonHelper.SerializeIndented(new
            {
                ViewId,
                ViewContent = ViewContent.ToDictionary(c => c.Key, c => ConvertToPropertyType(c.Key, c.Value)),
                ChildViews = ChildViews.Select(v => v.ToTemplate().Match(t => t, e => new object())).ToList()
            });

            return JsonHelper.DeserializeNotNull<TemplateComponent>(serialized, ignoreMissingMembers: true);
        }

        private void InitializeTypes()
        {
            foreach (var (key, config) in ViewContentProperties)
            {
                if (!Types.ContainsKey(key))
                {
                    Types[key] = config.PropertyType;
                }
            }
        }

        private void InitializeViewContent()
        {
            foreach (var (key, config) in ViewContentProperties)
            {
                if (!_viewContent.ContainsKey(key))
                {
                    _viewContent[key] = config.DefaultValue == null ? "" : config.DefaultValue.ToString() ?? "";
                }

                var propertyValue = config.Preprocess(_viewContent[key]);
                _viewContent[key] = ConvertToPropertyType(key, propertyValue)?.ToString() ?? "";
            }
        }

        private object? ConvertToPropertyType(string key, string value)
        {
            if (value == null)
            {
                return null;
            }

            var type = Types.ContainsKey(key)
                ? Types[key]
                : ViewContentPropertyType.String;

            return type switch
            {
                ViewContentPropertyType.String => value,
                ViewContentPropertyType.Bool => bool.TryParse(value, out var b) ? b : false,
                ViewContentPropertyType.Double => double.TryParse(value, out var d) ? d : 0.0,
                ViewContentPropertyType.Long => long.TryParse(value, out var l) ? l : 0,
                _ => JsonHelper.DeserializeOrNull<object>(value).Match(v => v, e => throw new Exception(e.Message))
            };
        }
    }
}
