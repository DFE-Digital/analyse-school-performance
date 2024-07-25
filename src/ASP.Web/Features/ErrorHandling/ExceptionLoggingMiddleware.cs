using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using ASP.Core.Logging;
using ASP.Core.Results;
using Microsoft.Extensions.Options;

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

            if (_options.ShowStackTrace)
            {
                httpContext.Items["Exception"] = exception;
            }

            await tableStorageProvider.AddTableEntry(tableStorageProblemDetails.Create(problemDetails))
                .Switch(
                    success => logger.LogInformation(success),
                    failure => logger.LogError(failure.ToString()));

            return false;
        }
    }
}