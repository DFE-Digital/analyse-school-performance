using ASP.Core.Results;

namespace ASP.Api
{
    public static class ResultExtensions
    {
        public static async Task<ApiResult> ToApiResultAsync<T>(this Task<Result<T>> resultTask, CancellationToken cancellationToken) where T : notnull
        {
            try
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    return new ApiResult(444, "");
                }

                var result = await resultTask;

                return result.ToApiResult(cancellationToken);
            } catch(Exception ex)
            {
                return new ApiResult(500, ex.Message);
            }
        }

        public static ApiResult ToApiResult<T>(this Result<T> result, CancellationToken cancellationToken) where T : notnull
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ApiResult(444, "");
            }

            return result.Match(
                r => new ApiResult(200, r),
                r => r switch {
                    MethodNotAllowedError e => new ApiResult(405, e.ToString()) { Headers = { { "Allow", string.Join(", ", e.AllowedMethods) } } },
                    NotFoundError e => new ApiResult(404, e.ToString()),
                    ValidationError e => new ApiResult(400, e.ToString()),
                    UnexpectedError e => new ApiResult(500, e.ToString()),
                    NotAllowedError e => new ApiResult(403, e.ToString()),
                    _ => new ApiResult(500, $@"Unhandled error type ""{r.GetType().FullName}"".")
                }
            );
        }
    }
}
