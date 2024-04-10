using ASP.Core.Templating;
using MR;

namespace ASP.Infrastructure.Repositories
{
    public class ContentTemplateDTO
    {
        public string id { get; set; } = null!;
        public string contentId { get; set; } = null!;
        public string? PageTitle { get; set; } = null!;
        public dynamic PageContent { get; set; } = new GracefulExpandoObject()!;
        public List<TemplateComponentDTO> Views { get; set; } = new List<TemplateComponentDTO>();

        internal ContentTemplate ToContentTemplate()
        {
            return new ContentTemplate(
                PageTitle,
                PageContent,
                (Views ?? new List<TemplateComponentDTO>())
                    .Select(v => v.ToTemplateComponent())
                    .ToList()
            );
        }

        public static ContentTemplateDTO FromContentTemplate(ContentTemplate template)
        {
            return new ContentTemplateDTO {
                PageTitle = template.PageTitle,
                PageContent = template.PageContent,
                Views = (template.Views ?? new List<TemplateComponent>())
                    .Select(TemplateComponentDTO.FromTemplateComponent)
                    .ToList()
            };
        }
    }
}
