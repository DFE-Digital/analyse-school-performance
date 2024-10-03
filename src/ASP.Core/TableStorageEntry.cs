using ASP.Core.Logging;
using Azure;
using Azure.Data.Tables;

namespace ASP.Core
{
    public class TableStorageEntry : ProblemDetails, ITableEntity
    {
        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}