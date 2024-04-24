namespace ASP.Web.Core.Templating
{
    public interface ITemplateComponentEditModelTypeLocator
    {
        Type FindEditModelType(string viewId);
        IEnumerable<Type> GetEditModelTypes();
    }
}
