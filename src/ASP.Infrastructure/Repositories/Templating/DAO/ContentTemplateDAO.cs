using ASP.Domain.Templating;
using MR;
using Newtonsoft.Json;

namespace ASP.Infrastructure.Repositories.Templating.DAO
{
    public class ContentTemplateDAO
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("contentId")]
        public string ContentId { get; set; } = "";

        [JsonProperty("isPublished")]
        public bool IsPublished { get; set; }

        public string PageTitle { get; set; } = "";

        public dynamic PageContent { get; set; } = new GracefulExpandoObject();

        public List<TemplateComponentDAO> Views { get; set; } = new List<TemplateComponentDAO>();

        internal ContentTemplate ToContentTemplate()
        {
            return new ContentTemplate(
                IsPublished,
                PageTitle,
                PageContent,
                (Views ?? new List<TemplateComponentDAO>())
                    .Select(v => v.ToTemplateComponent())
                    .ToList()
            );
        }

        public static ContentTemplateDAO FromContentTemplate(ContentTemplate template)
        {
            return new ContentTemplateDAO {
                IsPublished = template.IsPublished,
                PageTitle = template.PageTitle,
                PageContent = template.PageContent,
                Views = (template.Views ?? new List<TemplateComponent>())
                    .Select(TemplateComponentDAO.FromTemplateComponent)
                    .ToList()
            };
        }
    }
}
