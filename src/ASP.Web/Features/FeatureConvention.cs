using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace ASP.Web.Features
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature",
              GetFeatureName(controller.ControllerType));
        }

        private string GetFeatureName(TypeInfo controllerType)
        {
            return controllerType?.FullName?.Split('.')
              .SkipWhile(part => part != "Features")
              .Skip(1)
              .Take(1)
              .FirstOrDefault() ?? "";
        }
    }
}