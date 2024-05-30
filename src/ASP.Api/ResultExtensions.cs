using ASP.Core.Results;

namespace ASP.Api
{
    public static class ResultExtensions
    {
        public static async Task<ApiResult> ToApiResultAsync<T>(this Task<Result<T>> resultTask) where T : notnull
        {
            var result = await resultTask;

            return result.ToApiResult();
        }

        public static ApiResult ToApiResult<T>(this Result<T> result) where T : notnull
        {
            return result.Match(
                r => new ApiResult(200, r),
                r => r switch {
                    MethodNotAllowedError e => new ApiResult(405, e.Message) { Headers = { { "Allow", string.Join(", ", e.AllowedMethods) } } },
                    NotFoundError e => new ApiResult(404, e.Message),
                    ValidationError e => new ApiResult(400, e.Message),
                    UnexpectedError e => new ApiResult(500, e.Message),
                    _ => new ApiResult(500, $@"Unhandled error type ""{r.GetType().FullName}"".")
                }
            );
        }
    }
}
