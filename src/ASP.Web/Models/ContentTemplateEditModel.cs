using ASP.Core.Helpers;
using ASP.Core.PageContent;
using ErrorOr;

namespace ASP.Web.Models
{
    public class ContentTemplateEditModel
    {
        public string ContentId { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public List<TemplateComponentEditModel> Views { get; set; } = new();

        public ErrorOr<PageContentTemplate> ToTemplate()
        {
            var serialized = JsonHelper.SerializeIndented(new {
                ContentId,
                PageTitle,
                Views = Views.Select(v => v.ToTemplate().MatchFirst(t => t, e => new object())).ToList()
            });

            return JsonHelper.DeserializeIgnoringMissingMembers<PageContentTemplate>(serialized);
        }

        public static ContentTemplateEditModel FromTemplate(string contentId, PageContentTemplate template)
        {
            var views = template.Views ?? new List<PageContentTemplateView>();
            return new ContentTemplateEditModel
            {
                ContentId = contentId,
                PageTitle = template.PageTitle ?? "",
                Views = views.Select(TemplateComponentEditModel.Create).ToList()
            };
        }
    }
}
