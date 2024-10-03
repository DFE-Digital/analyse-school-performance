using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using ASP.Core.Logging;
using ASP.Core.Results;
using Microsoft.Extensions.Options;
using ASP.Web.Core.ErrorHandling;
using ASP.Core;

namespace ASP.Web.Features.ErrorHandling
{
    public class ExceptionLoggingMiddleware(
        ITableStorageProvider tableStorageProvider,
        ILogger<ExceptionLoggingMiddleware> logger,
        IOptions<ErrorHandlingOptions> options
    ) : IExceptionHandler
    {
        private readonly ErrorHandlingOptions _options = (options ?? throw new ArgumentNullException(nameof(options)))
            .Value;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            var tableStorageProblemDetails = new TableStorageProblemDetails(httpContext, HttpStatusCode.InternalServerError.ToString());

            var problemDetails = new ProblemDetails()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = exception.StackTrace,
            };

            var entry = tableStorageProblemDetails.Create(problemDetails);
            await tableStorageProvider.AddTableEntry(entry)
                .Switch(
                    _ => logger.LogInformation("Error details added to table storage with error code " + entry.RowKey),
                    failure => logger.LogError(failure.ToString()));

            string errorMessage = exception.Message;
            string? stackTrace = exception.StackTrace;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                errorMessage += " " + exception.Message;
                stackTrace += " " + exception.StackTrace;
            }

            httpContext.Items["ErrorMessage"] = errorMessage;
            if (_options.ShowStackTrace)
            {
                httpContext.Items["StackTrace"] = stackTrace;
            }

            return false;
        }
    }
}