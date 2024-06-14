using ASP.Core.Templating;
using MR;

namespace ASP.Infrastructure.Repositories
{ 
    public class TemplateComponentDTO
    {
        public string ViewId { get; set; } = null!;
        public dynamic ViewContent { get; set; } = new GracefulExpandoObject()!;
        public dynamic ViewModel { get; set; } = new GracefulExpandoObject()!;
        public List<TemplateComponentDTO> ChildViews { get; set; } = new List<TemplateComponentDTO>();

        internal TemplateComponent ToTemplateComponent()
        {
            return new TemplateComponent(
                ViewId,
                ViewContent,
                ViewModel,
                (ChildViews ?? new List<TemplateComponentDTO>())
                    .Select(v => v.ToTemplateComponent())
                    .ToList()
            );
        }

        public static TemplateComponentDTO FromTemplateComponent(TemplateComponent component)
        {
            return new TemplateComponentDTO {
                ViewId = component.ViewId,
                ViewContent = component.ViewContent,
                ViewModel = component.ViewModel,
                ChildViews = (component.ChildViews ?? new List<TemplateComponent>())
                    .Select(FromTemplateComponent)
                    .ToList()
            };
        }
    }
}
