using ASP.Application;
using ASP.Core.Extensions;
using ASP.Core.Helpers;
using ASP.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Api
{
    public static class ResultExtensions
    {
        public static async Task<ActionResult> ToApiResultAsync<T>(this Task<Result<T>> resultTask, ErrorHandlingOptions options, CancellationToken cancellationToken) where T : notnull
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ApiResult(444, "");
                }

                var result = await resultTask;

                return result.ToApiResult(options, cancellationToken);
            }
            catch (Exception ex)
            {
                return new ApiResult(500, ex.Message);
            }
        }

        public static ActionResult ToApiResult<T>(this Result<T> result, ErrorHandlingOptions options, CancellationToken cancellationToken) where T : notnull
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ApiResult(444, "");
            }

            return result.Match(
               r => r is FileStreamResponse fileResult
                    ? (ActionResult)new FileApiResult(fileResult.Content, fileResult.ContentType, fileResult.FileName)
                    : new ApiResult(200, r),
                r => r switch
                {
                    MethodNotAllowedError e => new ApiResult(405, e.ToString()) { Headers = { { "Allow", string.Join(", ", e.AllowedMethods) } } },
                    NotFoundError e => new ApiResult(404, e.ToString()),
                    ValidationError e => new ApiResult(400, e.ToString().ReplacePrefix("Invalid: ", "Bad request: ")),
                    UnexpectedError e => new ApiResult(500, JsonHelper.Serialize(options.ShowStackTrace ? e : Error.Unexpected(e.Message, null))),
                    NotAllowedError e => new ApiResult(403, e.ToString()),
                    _ => new ApiResult(500, $@"Unhandled error type ""{r.GetType().FullName}"".")
                }
            );
        }
    }
}
