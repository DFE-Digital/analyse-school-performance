using Azure;
using Azure.Data.Tables;

namespace ASP.Web.ExceptionHandlers
{
    public class AppException : ITableEntity
    {
        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public int? StatusCode { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Detail { get; set; }
    }

}