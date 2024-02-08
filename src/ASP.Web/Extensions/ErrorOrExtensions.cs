using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Extensions
{
    public static class ErrorOrExtensions
    {
        public static IActionResult ToActionResult<T>(this ErrorOr<T> errorOr, Func<T, IActionResult> action)
        {
            return errorOr
                .MatchFirst(
                    response => action(response),
                    error => error.Type switch {
                        ErrorType.NotFound => new ObjectResult(error.Description) { StatusCode = 404 },
                        _ => new ObjectResult(error.Description) { StatusCode = 500 }
                    }
                );
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<ErrorOr<T>> errorOr, Func<T, IActionResult> action)
        {
            return await errorOr
                .MatchFirst(
                    response => action(response),
                    error => error.Type switch {
                        ErrorType.NotFound => new ObjectResult(error.Description) { StatusCode = 404 },
                        _ => new ObjectResult(error.Description) { StatusCode = 500 }
                    }
                );
        }
    }
}