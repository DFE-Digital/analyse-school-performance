using ASP.Domain.Templating;
using MR;

namespace ASP.Domain.Repositories.Templating.DAO
{
    public class TemplateComponentDAO
    {
        public string ViewId { get; set; } = null!;
        public dynamic ViewContent { get; set; } = new GracefulExpandoObject()!;
        public dynamic ViewModel { get; set; } = new GracefulExpandoObject()!;
        public List<TemplateComponentDAO> ChildViews { get; set; } = new List<TemplateComponentDAO>();

        internal TemplateComponent ToTemplateComponent()
        {
            return new TemplateComponent(
                ViewId,
                ViewContent,
                ViewModel,
                (ChildViews ?? new List<TemplateComponentDAO>())
                    .Select(v => v.ToTemplateComponent())
                    .ToList()
            );
        }

        public static TemplateComponentDAO FromTemplateComponent(TemplateComponent component)
        {
            return new TemplateComponentDAO {
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
