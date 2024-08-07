using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ASP.Web;

public static class HttpContextExtensions
{
    /// <summary>
    /// This extension method enables an HttpContext to asynchronously render a view with a specified model. 
    /// It retrieves necessary services from the context, constructs a ViewResult with the provided model and view name, 
    /// and then executes it using an IActionResultExecutor&lt;ViewResult&gt;, effectively generating the view's output.
    /// 
    /// It is utilized in the CustomPageNotFoundMiddleware.cs file as follows:
    /// 
    /// <code>
    /// await context.RenderViewAsync("PageNotFoundError", errorViewModel);
    /// </code>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    public static async Task RenderViewAsync(this HttpContext context, string viewName, object model)
    {
        var services = context.RequestServices;
        var executor = services.GetRequiredService<IActionResultExecutor<ViewResult>>();

        var viewResult = new ViewResult()
        {
            ViewName = viewName,
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            },
            TempData = new TempDataDictionary(context, services.GetRequiredService<ITempDataProvider>())
        };

        var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
        await executor.ExecuteAsync(actionContext, viewResult);
    }
}