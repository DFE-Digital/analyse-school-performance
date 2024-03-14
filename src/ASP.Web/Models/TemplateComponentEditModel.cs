using ASP.Core.PageContent;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ASP.Core.Helpers;
using ErrorOr;
using System.Reflection;

namespace ASP.Web.Models
{
    public abstract class TemplateComponentEditModel
    {
        private static readonly Dictionary<string, ViewContentPropertyConfig> _viewContentProperties
            = new Dictionary<string, ViewContentPropertyConfig>();

        public TemplateComponentEditModel()
        {
            InitializeTypes();
        }

        public TemplateComponentEditModel(PageContentTemplateView contentTemplate)
        {
            IDictionary<string, object> viewContent = contentTemplate.ViewContent ?? new Dictionary<string, object>();
            IList<PageContentTemplateView> childViews = contentTemplate.ChildViews ?? new List<PageContentTemplateView>();

            ViewId = contentTemplate.ViewId;
            ViewContent = viewContent.ToDictionary(c => c.Key, c =>
                c.Value switch {
                    null => "",
                    string s => s,
                    bool b => JsonConvert.SerializeObject(b),
                    double d => d.ToString(),
                    long l => l.ToString(),
                    JArray a => JsonConvert.SerializeObject(a, Formatting.Indented),
                    JObject o => JsonConvert.SerializeObject(o, Formatting.Indented),
                    _ => c.Value.ToString() ?? ""
                });
            ChildViews = childViews.Select(Create).ToList();

            InitializeTypes();

            foreach (var (key, value) in viewContent)
            {
                if (!Types.ContainsKey(key))
                {
                    Types[key] = value switch {
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

        public ErrorOr<PageContentTemplateView> ToTemplate()
        {
            var serialized = JsonHelper.SerializeIndented(new {
                ViewId,
                ViewContent = ViewContent.ToDictionary(c => c.Key, c => ConvertToPropertyType(c.Key, c.Value)),
                ChildViews = ChildViews.Select(v => v.ToTemplate().MatchFirst(t => t, e => new object())).ToList()
            });

            return JsonHelper.DeserializeIgnoringMissingMembers<PageContentTemplateView>(serialized);
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
                _viewContent[key] = ConvertToPropertyType(key, propertyValue).ToString() ?? "";
            }
        }

        private object ConvertToPropertyType(string key, string value)
        {
            var type = Types.ContainsKey(key)
                ? Types[key]
                : ViewContentPropertyType.String;

            return type switch {
                ViewContentPropertyType.String => value,
                ViewContentPropertyType.Bool => bool.TryParse(value, out var b) ? b : false,
                ViewContentPropertyType.Double => double.TryParse(value, out var d) ? d : 0.0,
                ViewContentPropertyType.Long => long.TryParse(value, out var l) ? l : 0,
                _ => JsonHelper.Deserialize<object>(value).MatchFirst(v => v, e => null!)
            };
        }

        public static TemplateComponentEditModel Create(PageContentTemplateView v)
        {
            var editModelType = FindEditModelType(typeof(TemplateComponentEditModel).Assembly.GetTypes(), v.ViewId);

            return (TemplateComponentEditModel) Activator.CreateInstance(editModelType, v)!;
        }

        public static Type FindEditModelType(IEnumerable<Type> types, string viewId)
        {
            var templateComponentEditModelType = typeof(TemplateComponentEditModel);

            return types
                .Where(t => t != templateComponentEditModelType && t.IsAssignableTo(templateComponentEditModelType))
                .FirstOrDefault(t => t.GetCustomAttribute<EditModelForAttribute>()?.ViewId == viewId)
                ?? typeof(GenericTemplateComponentEditModel);
        }
    }
}
