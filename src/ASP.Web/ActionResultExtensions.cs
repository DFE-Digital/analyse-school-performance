using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result) where T : IActionResult
        {
            return result
                .Match(
                    response => (IActionResult)response,
                    error => error switch
                    {
                        NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = 404 },
                        _ => new ObjectResult(error.ToString()) { StatusCode = 500 }
                    }
                );
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result) where T : IActionResult
        {
            return await result
                .Match(
                    response => (IActionResult)response,
                    error => error switch
                    {
                        NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = 404 },
                        _ => new ObjectResult(error.ToString()) { StatusCode = 500 }
                    }
                );
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> action)
        {
            return result
                .Match(
                    response => action(response),
                    error => error switch
                    {
                        NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = 404 },
                        _ => new ObjectResult(error.ToString()) { StatusCode = 500 }
                    }
                );
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> action, T defaultIfNotFound)
        {
            return result
                .Match(
                    response => action(response),
                    error => error switch
                    {
                        NotFoundError e => action(defaultIfNotFound),
                        _ => new ObjectResult(error.ToString()) { StatusCode = 500 }
                    }
                );
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result, Func<T, IActionResult> action,
            IHostEnvironment hostEnvironment)
        {
           return await result.Match(
                response => action(response),
                error => {
                    // Check if the application is in development mode
                    if (hostEnvironment.IsDevelopment())
                    {
                        // Development-specific error handling
                        return error switch
                        {
                            NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = StatusCodes.Status404NotFound },
                            _ => new ObjectResult(error.ToString()) { StatusCode = StatusCodes.Status500InternalServerError }
                        };
                    }
                    else
                    {
                        // Production error handling
                        // For non-dev environments, display a 'Page Not Found' (404) page using `StatusCodeResult`.
                        // This ensures proper utilization of `UseStatusCodePagesWithReExecute`.
                        // `UseStatusCodePagesWithReExecute` will not trigger if the response body has already started to be written.
                        // Ensure that our `ToActionResult` method does not write to the response body directly for non-dev environments.
                        // Instead, it should only set the status code.
                        return error switch
                        {
                            NotFoundError e => new StatusCodeResult(StatusCodes.Status404NotFound), 
                            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError) 
                        };
                    }
                }
            );
        }
        
        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> action,
            IHostEnvironment hostEnvironment)
        {
            return result.Match(
                response => action(response),
                error => {
                    // Check if the application is in development mode
                    if (hostEnvironment.IsDevelopment())
                    {
                        // Development-specific error handling
                        return error switch
                        {
                            NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = StatusCodes.Status404NotFound },
                            _ => new ObjectResult(error.ToString()) { StatusCode = StatusCodes.Status500InternalServerError }
                        };
                    }
                    else
                    {
                        // Production error handling
                        // For non-dev environments, display a 'Page Not Found' (404) page using `StatusCodeResult`.
                        // This ensures proper utilization of `UseStatusCodePagesWithReExecute`.
                        // `UseStatusCodePagesWithReExecute` will not trigger if the response body has already started to be written.
                        // Ensure that our `ToActionResult` method does not write to the response body directly for non-dev environments.
                        // Instead, it should only set the status code.
                        return error switch
                        {
                            NotFoundError e => new StatusCodeResult(StatusCodes.Status404NotFound), 
                            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError) 
                        };
                    }
                }
            );
        }
        
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result, Func<T, IActionResult> action, T defaultIfNotFound)
        {
            return await result
                .Match(
                    response => action(response),
                    error => error switch
                    {
                        NotFoundError e => action(defaultIfNotFound),
                        _ => new ObjectResult(error.ToString()) { StatusCode = 500 }
                    }
                );
        }
    }
}