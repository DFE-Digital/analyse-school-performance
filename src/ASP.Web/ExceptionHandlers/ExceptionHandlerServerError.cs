using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ASP.Web.ExceptionHandlers
{
    public class ExceptionHandlerServerError(ITableStorageProvider tableStorageProvider, 
        ILogger<ExceptionHandlerServerError> logger) : ExceptionHandlerBase, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {                                                   
                var tableStorageEntry = CreateTableStorageEntry(httpContext, exception,
                    HttpStatusCode.InternalServerError.ToString(),
                    (int)HttpStatusCode.InternalServerError);

               var result =  await tableStorageProvider.UpdateTable(tableStorageEntry);
               result.Switch(
                    success => logger.LogInformation(success.ReasonPhrase),
                    failure => logger.LogError(failure.Message));
            }

            return false;
        }
    }
}
