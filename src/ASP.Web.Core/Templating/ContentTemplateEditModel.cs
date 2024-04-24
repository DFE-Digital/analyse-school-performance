using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Templating;

namespace ASP.Web.Core.Templating
{
    public class ContentTemplateEditModel
    {
        public string ContentId { get; set; } = "";
        public string PageTitle { get; set; } = "";
        public List<TemplateComponentEditModel> Views { get; set; } = new();

        public Result<ContentTemplate> ToTemplate()
        {
            var serialized = JsonHelper.SerializeIndented(new
            {
                ContentId,
                PageTitle,
                Views = Views.Select(v => v.ToTemplate().Match(t => t, e => new object())).ToList()
            });

            return JsonHelper.DeserializeIgnoringMissingMembers<ContentTemplate>(serialized);
        }

        public static ContentTemplateEditModel FromTemplate(string contentId, ContentTemplate template, ITemplateComponentEditModelFactory editModelFactory)
        {
            var views = template.Views ?? new List<TemplateComponent>();
            return new ContentTemplateEditModel
            {
                ContentId = contentId,
                PageTitle = template.PageTitle ?? "",
                Views = views.Select(editModelFactory.CreateTemplateComponentEditModel).ToList()
            };
        }
    }
}
