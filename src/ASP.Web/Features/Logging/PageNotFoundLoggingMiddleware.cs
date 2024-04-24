namespace ASP.Web.Features.Logging;

public class PageNotFoundLoggingMiddleware
{
    private readonly ILogger<PageNotFoundLoggingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public PageNotFoundLoggingMiddleware(ILogger<PageNotFoundLoggingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    /// <summary>
    /// This middleware method processes HTTP requests, logs specific information, and executes the next middleware in the pipeline. 
    /// It also checks for 404 errors. If a 404 error occurs, it constructs and logs the full request URL for diagnostic purposes. 
    /// Additionally, it catches and logs any exceptions that occur during processing to ensure errors are not silently ignored.
    /// This functionality provides useful insights, such as identifying the URL that returned a 404 not found error.
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            if (context.Response.StatusCode == 404)
            {
                var scheme = context.Request.Scheme;
                var host = context.Request.Headers.ContainsKey("X-Forwarded-Host")
                    ? context.Request.Headers["X-Forwarded-Host"].ToString()
                    : context.Request.Host.ToString();
                var path = context.Request.Path;
                var queryString = context.Request.QueryString;
                var url = $"{scheme}://{host}{path}{queryString}";
                _logger.LogInformation("404 Response for {url}", url);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exception in HTTP pipeline");
            throw;
        }
    }
}