using ASP.Core.Logging;
using Microsoft.AspNetCore.Http;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProblemDetails(HttpContext httpContext, string partitionKey) : IProblemDetails<TableStorageEntry>
    {
        private readonly HttpContext _httpContext = httpContext;
        private readonly string _partitionKey = partitionKey;

        public TableStorageEntry Create(ProblemDetails problemDetails)
        {
            var tableStorageEntry = new TableStorageEntry {
                RowKey = _httpContext.TraceIdentifier,
                PartitionKey = _partitionKey,
                StatusCode = problemDetails.StatusCode,
                Detail = problemDetails.Detail,
                Title = problemDetails.Title,
                Type = problemDetails.Type,
            };

            return tableStorageEntry;
        }
    }
}
