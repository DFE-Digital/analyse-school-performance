using ASP.Core.Helpers;
using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, IHostEnvironment hostEnvironment) where T : IActionResult
        {
            return result.Match(
                response => response,
                error => HandleError(error, hostEnvironment)
            );
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> action, IHostEnvironment hostEnvironment)
        {
            return result.Match(
                response => action(response),
                error => HandleError(error, hostEnvironment)
            );
        }

        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> action, T defaultIfNotFound, IHostEnvironment hostEnvironment)
        {
            return result
                .DefaultIf(e => e is NotFoundError, defaultIfNotFound)
                .Match(
                    response => action(response),
                    error => HandleError(error, hostEnvironment)
                );
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask, IHostEnvironment hostEnvironment) where T : IActionResult
        {
            var result = await resultTask;

            return result.ToActionResult(hostEnvironment);
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask, Func<T, IActionResult> action, IHostEnvironment hostEnvironment)
        {
            var result = await resultTask;

            return result.ToActionResult(action, hostEnvironment);
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask, Func<T, IActionResult> action, T defaultIfNotFound, IHostEnvironment hostEnvironment)
        {
            var result = await resultTask;

            return result.ToActionResult(action, defaultIfNotFound, hostEnvironment);
        }

        private static IActionResult HandleError(Error error, IHostEnvironment hostEnvironment)
        {
            // Check if the application is in development mode
            if (hostEnvironment.IsDevelopment())
            {
                // Development-specific error handling
                return error switch {
                    NotFoundError e => new ObjectResult(e.ToString()) { StatusCode = StatusCodes.Status404NotFound },
                    NotAllowedError e => new ObjectResult(e.ToString()) { StatusCode = StatusCodes.Status403Forbidden },
                    UnexpectedError e => new ObjectResult(JsonHelper.Serialize(e)) { StatusCode = StatusCodes.Status500InternalServerError },
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
                return error switch {
                    NotFoundError => new StatusCodeResult(StatusCodes.Status404NotFound),
                    NotAllowedError => new StatusCodeResult(StatusCodes.Status403Forbidden),
                    _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
                };
            }
        }
    }
}