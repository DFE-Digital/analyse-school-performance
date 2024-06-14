using ASP.Core.Logging;
using ASP.Infrastructure.TableStorage;
using System.Net;

namespace ASP.Web.Features.Logging;

public class PageNotFoundLoggingMiddleware
{
    private readonly ITableStorageProvider _tableStorageProvider;
    private readonly ILogger<PageNotFoundLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public PageNotFoundLoggingMiddleware(ITableStorageProvider tableStorageProvider,
        ILogger<PageNotFoundLoggingMiddleware> logger, RequestDelegate next)
    {
        _tableStorageProvider = tableStorageProvider;
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// This middleware method processes HTTP requests, logs specific information, and executes the next middleware in the pipeline. 
    /// It also checks for 404 errors. If a 404 error occurs, it constructs and the full request URL and writes to table storrage for diagnostic purposes. 
    /// Additionally, it catches and logs any exceptions that occur during processing to ensure errors are not silently ignored.
    /// This functionality provides useful insights, such as identifying the URL that returned a 404 not found error.
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        await _next(httpContext);

        if (!httpContext.Request.Path.Equals("/error/", StringComparison.OrdinalIgnoreCase) &&
            httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
        {
            var scheme = httpContext.Request.Scheme;
            var host = httpContext.Request.Headers.ContainsKey("X-Forwarded-Host")
                ? httpContext.Request.Headers["X-Forwarded-Host"].ToString()
                : httpContext.Request.Host.ToString();
            var path = httpContext.Request.Path;
            var queryString = httpContext.Request.QueryString;
            var url = $"{scheme}://{host}{path}{queryString}";

            var problemDetails = new ProblemDetails()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Type = "Error",
                Title = HttpStatusCode.NotFound.ToString(),
                Detail = url,
            };

            var tableStorageProblemDetails = new TableStorageProblemDetails(httpContext, HttpStatusCode.NotFound.ToString());

            var result = await _tableStorageProvider.AddTableEntry(tableStorageProblemDetails.Create(problemDetails));
            result.Switch(
                success => _logger.LogInformation(success),
                failure => _logger.LogError(failure.ToString()));
        }
    }
}