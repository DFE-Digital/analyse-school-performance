using ASP.Core.Exceptions;
using ASP.Core.Logging;
using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ASP.Web.ExceptionHandlers
{
    public class ExceptionHandlerServerError(ITableStorageProvider tableStorageProvider,
        ILogger<ExceptionHandlerServerError> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                var tableStorageProblemDetails = new TableStorageProblemDetails(httpContext, HttpStatusCode.InternalServerError.ToString());

                var problemDetails = new ProblemDetails()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = exception.Message,
                    Detail = exception.StackTrace,
                };

                var result = await tableStorageProvider.AddTableEntry(tableStorageProblemDetails.Create(problemDetails));
                result.Switch(
                    success => logger.LogInformation(success),
                    failure => logger.LogError(failure.Message));
            }

            return false;
        }
    }
}
