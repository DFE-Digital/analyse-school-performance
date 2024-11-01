using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP.Web.Extensions;

public static class ModelStateDictionaryExtensions
{
    public static bool HasError(this ModelStateDictionary? modelState, string key)
    {
        if (modelState == null) return false;
        var hasError = modelState.TryGetValue(key, out var entry) && 
               entry.Errors.Count > 0;
        return hasError;
    }
    
    public static ModelError? GetFirstErrorForKey(this ModelStateDictionary modelState, string key)
    {
        return modelState
            .FirstOrDefault(x => x.Key == key)
            .Value?.Errors.FirstOrDefault();
    }
}
