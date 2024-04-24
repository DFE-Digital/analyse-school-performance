using ASP.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.ErrorHandling
{
    public class CustomPageNotFoundMiddleware
    {
        private readonly ILogger<CustomPageNotFoundMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CustomPageNotFoundMiddleware(ILogger<CustomPageNotFoundMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// This middleware method intercepts HTTP responses to identify 404 errors containing the specific string "404," 
        /// and then customizes the page not found error page based on the response content. It captures the response in a memory stream, 
        /// enabling inspection and modification before restoring the original stream. This ensures response integrity for downstream processing and client delivery. 
        /// The middleware resolves the issue where a request for a URL results in a 404 not found error due to a DB error, as exemplified in the provided response snippet.
        /// // The error message is truncated due to its length. Only a portion of the message is provided for brevity.
        /// /// <code>
        /// Response status code does not indicate success: NotFound (404);
        /// Substatus: 0; ActivityId: 11bc9aa3-ab99-47c7-a2f0-f827136d46c9;  Reason: (code : NotFound)
        /// </code>
        /// This addresses one of the story points outlined in #195086, which specifies displaying an error message with details exclusively in the development environment.
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                // Redirects the response output to the newly created memory stream,
                // allowing the middleware to capture and potentially modify the response written by downstream middleware and endpoints.
                context.Response.Body = memoryStream;

                // Asynchronously executes the remaining middleware in the pipeline,
                // allowing them to write to the memoryStream instead of the original response body.
                await _next(context);

                memoryStream.Position = 0;

                // Creates a new StreamReader that reads all the contents of the memory stream into a string.
                // This allows the middleware to inspect or log the response body text.
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                // Checks if the status code of the response is 404.
                // If the condition is met, it assumes an error has occurred that should be handled.
                if (context.Response.StatusCode == 404)
                {
                    // Prepare the custom error response directly here
                    context.Response.Body = originalBodyStream;
                    context.Response.ContentType = "text/html";
                    var errorViewModel = new ErrorViewModel {
                        ErrorMessage = responseBody
                    };

                    await context.RenderViewAsync("~/Features/ErrorHandling/PageNotFoundError.cshtml", errorViewModel);
                    return;
                }

                // Reset the memory stream and write back to the original body stream
                memoryStream.Position = 0;

                // Copies all contents of the memoryStream back to the originalBodyStream,
                // ensuring that the response sent to the client includes any modifications or inspections performed by this middleware.
                await memoryStream.CopyToAsync(originalBodyStream);
            }
            // Restores the original response body stream to ensure that further middleware
            // or the application's response handling mechanism uses the correct stream.
            context.Response.Body = originalBodyStream;
        }
    }
}