using ASP.Core.Helpers;
using ASP.Core.Templating;
using ErrorOr;

namespace ASP.Web.Models
{
    public class ContentTemplateEditModel
    {
        public string ContentId { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public List<TemplateComponentEditModel> Views { get; set; } = new();

        public ErrorOr<ContentTemplate> ToTemplate()
        {
            var serialized = JsonHelper.SerializeIndented(new {
                ContentId,
                PageTitle,
                Views = Views.Select(v => v.ToTemplate().MatchFirst(t => t, e => new object())).ToList()
            });

            return JsonHelper.DeserializeIgnoringMissingMembers<ContentTemplate>(serialized);
        }

        public static ContentTemplateEditModel FromTemplate(string contentId, ContentTemplate template)
        {
            var views = template.Views ?? new List<TemplateComponent>();
            return new ContentTemplateEditModel
            {
                ContentId = contentId,
                PageTitle = template.PageTitle ?? "",
                Views = views.Select(TemplateComponentEditModel.Create).ToList()
            };
        }
    }
}
