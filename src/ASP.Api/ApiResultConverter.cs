using ASP.Api.Client;
using ASP.Core.Network;
using ASP.Core.Results;
using ASP.Core.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP.Api;

public class ApiResultConverter
{
    private readonly ErrorHandlingOptions _options;

    public ApiResultConverter(IOptions<ErrorHandlingOptions> options)
    {
        _options = options?.Value
            ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<ActionResult> ConvertToApiResultAsync<T>(Task<Result<T>> resultTask, CancellationToken cancellationToken)
        where T : notnull
    {
        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new ApiResult(444, "");
            }

            var result = await resultTask;

            return ConvertToApiResult(result, cancellationToken);
        }
        catch (Exception ex)
        {
            return new ApiResult(500, ex.Message);
        }
    }

    public ActionResult ConvertToApiResult<T>(Result<T> result, CancellationToken cancellationToken)
        where T : notnull
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return new ApiResult(444, "");
        } 
        return result.Match(
            r => r is FileStreamResponse fileResult
                ? (ActionResult)new FileApiResult(fileResult.Content, fileResult.ContentType, fileResult.FileName)
                : new ApiResult(200, r),
            r => r switch {
                MethodNotAllowedError e => new ApiResult(405, e.ToString()) { Headers = { { "Allow", string.Join(", ", e.AllowedMethods) } } },
                NotFoundError e => new ApiResult(404, e.ToString()),
                ValidationError e => new ApiResult(400, e.ToString().ReplacePrefix("Invalid: ", "Bad request: ")),
                UnexpectedError e => new ApiResult(500, JsonHelper.Serialize(_options.ShowStackTrace ? e : Error.Unexpected(e.Message, null))),
                NotAllowedError e => new ApiResult(403, e.ToString()),
                _ => new ApiResult(500, $@"Unhandled error type ""{r.GetType().FullName}"".")
            }
        );
    }
}
