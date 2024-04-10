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
                        NotFoundError e => new ObjectResult(e.Message) { StatusCode = 404 },
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
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
                        NotFoundError e => new ObjectResult(e.Message) { StatusCode = 404 },
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
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
                        NotFoundError e => new ObjectResult(e.Message) { StatusCode = 404 },
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
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
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
                    }
                );
        }

        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> result, Func<T, IActionResult> action)
        {
            return await result
                .Match(
                    response => action(response),
                    error => error switch
                    {
                        NotFoundError e => new ObjectResult(e.Message) { StatusCode = 404 },
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
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
                        _ => new ObjectResult(error.Message) { StatusCode = 500 }
                    }
                );
        }
    }
}