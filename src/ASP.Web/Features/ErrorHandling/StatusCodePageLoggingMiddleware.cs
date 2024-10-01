using ASP.Core.Helpers;
using ASP.Core.Logging;
using ASP.Core.Results;
using ASP.Infrastructure.TableStorage;
using ASP.Web.Core.Environment;
using ASP.Web.Core.ErrorHandling;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System.Net;

namespace ASP.Web.Features.ErrorHandling
{
    public class StatusCodePageLoggingMiddleware
    {
        private readonly ILogger<StatusCodePageLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ITableStorageProvider _tableStorageProvider;
        private readonly ErrorHandlingOptions _options;

        public StatusCodePageLoggingMiddleware(
            ILogger<StatusCodePageLoggingMiddleware> logger, 
            RequestDelegate next, 
            IHostEnvironment hostEnvironment, 
            ITableStorageProvider tableStorageProvider,
            IOptions<ErrorHandlingOptions> options
        )
        {
            _logger = logger;
            _next = next;
            _hostEnvironment = hostEnvironment;
            _tableStorageProvider = tableStorageProvider;
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        /// <summary>
        /// This middleware method intercepts HTTP responses to identify 404 errors" 
        /// and then customizes the page not found error page based on the response content. It captures the response in a memory stream, 
        /// enabling inspection and modification before restoring the original stream. This ensures response integrity for downstream processing and client delivery. 
        /// The middleware resolves the issue where a request for a URL results in a 404 not found error due to a DB error, as exemplified in the provided response snippet.
        /// The error message is truncated due to its length. Only a portion of the message is provided for brevity.
        /// <code>
        /// Response status code does not indicate success: NotFound (404);
        /// Substatus: 0; ActivityId: 11bc9aa3-ab99-47c7-a2f0-f827136d46c9;  Reason: (code : NotFound)
        /// </code>
        /// This addresses one of the story points outlined in #195086, which specifies displaying an error message with details exclusively in the development environment.
        /// It also checks for 404 errors. If a 404 error occurs, it constructs and the full request URL and writes to table storrage for diagnostic purposes. 
        /// Additionally, it catches and logs any exceptions that occur during processing to ensure errors are not silently ignored.
        /// This functionality provides useful insights, such as identifying the URL that returned a 404 not found error.
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            string body = "";
            using (var memoryStream = new MemoryStream())
            using (var memoryStreamReader = new StreamReader(memoryStream))
            {
                var originalBodyStream = context.Response.Body;

                // Redirects the response output to the newly created memory stream,
                // allowing the middleware to capture and potentially modify the response written by downstream middleware and endpoints.
                context.Response.Body = memoryStream;

                // Asynchronously executes the remaining middleware in the pipeline,
                // allowing them to write to the memoryStream instead of the original response body.
                await _next(context);

                // Reset the new stream back to the beginning so we can read the contents
                memoryStream.Position = 0;

                // Prepare the custom error response directly here
                context.Response.Body = originalBodyStream;

                // Handle Developer Exception page
                if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    // Getting the first line of the response body to check
                    var firstLine = await memoryStreamReader.ReadLineAsync();

                    // Reset the stream back to the beginning so we can read the contents again
                    memoryStream.Position = 0;

                    // If the response is already HTML then the error has been handled by the ExceptionHandler
                    // or the Developer Exception Page, just output that HTML and we're done.
                    if (firstLine != null && firstLine.StartsWith("<!DOCTYPE"))
                    {
                        // Copies all contents of the memoryStream to the response body, ensuring that the response sent to the client
                        // includes any modifications or inspections performed by downstream middleware.
                        await memoryStream.CopyToAsync(context.Response.Body);
                        return;
                    }
                }

                // Handle non-error responses
                if (context.Request.Path.StartsWithSegments("/error/", StringComparison.OrdinalIgnoreCase) ||
                    (string)(context.Request.RouteValues["controller"] ?? "") == "Error" ||
                    context.Response.StatusCode == (int)HttpStatusCode.OK ||
                    context.Response.StatusCode == (int)HttpStatusCode.NotModified ||
                    context.Response.StatusCode == (int)HttpStatusCode.MovedPermanently ||
                    context.Response.StatusCode == (int)HttpStatusCode.Redirect)
                {
                    // Copies all contents of the memoryStream to the response body, ensuring that the response sent to the client
                    // includes any modifications or inspections performed by downstream middleware.
                    await memoryStream.CopyToAsync(context.Response.Body);
                    return;
                }

                body = await memoryStreamReader.ReadToEndAsync();
            }

            (string errorMessage, string? stackTrace) = context.Response.StatusCode switch {
                500 => JsonHelper.DeserializeNotNull<UnexpectedError>(body)
                    .Match(e => (e.Message, e.StackTrace), _ => (body, null)),
                _ => (body, null)
            };

            var scheme = context.Request.Scheme;
            var host = context.Request.Headers.ContainsKey("X-Forwarded-Host")
                ? context.Request.Headers["X-Forwarded-Host"].ToString()
                : context.Request.Host.ToString();
            var path = context.Request.Path;
            var queryString = context.Request.QueryString;
            var url = $"{scheme}://{host}{path}{queryString}";

            var problemDetails = new ProblemDetails {
                StatusCode = context.Response.StatusCode,
                Type = context.Response.StatusCode.ToString(),
                Title = errorMessage,
                Detail = url,
            };

            var tableStorageProblemDetails = new TableStorageProblemDetails(context, context.Response.StatusCode.ToString());

            await _tableStorageProvider.AddTableEntry(tableStorageProblemDetails.Create(problemDetails))
                .Switch(
                    success => _logger.LogInformation(success),
                    failure => _logger.LogError(failure.ToString())
                );

            if (_hostEnvironment.ShouldShowErrorMessage())
            { 
                context.Items["ErrorMessage"] = errorMessage;
                if (_options.ShowStackTrace)
                {
                    context.Items["StackTrace"] = stackTrace;
                }
            }

            // Continue execution with the ErrorController
            context.Request.Path = context.Response.StatusCode switch {
                (int)HttpStatusCode.NotFound => "/error/pagenotfound/",
                (int)HttpStatusCode.Forbidden => "/error/accessdenied/",
                _ => "/error/servererror/"
            };
            context.Request.QueryString = new QueryString();

            // Reset content type and route values that were set from the initial pass
            context.Response.ContentType = null;
            context.SetEndpoint(endpoint: null);
            var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
            if (routeValuesFeature != null)
            {
                routeValuesFeature.RouteValues = null!;
            }

            await _next(context);

            // Reset back to original path
            context.Request.QueryString = queryString;
            context.Request.Path = path;
        }
    }
}