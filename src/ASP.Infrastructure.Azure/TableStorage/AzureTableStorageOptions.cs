using ASP.Core;
using ASP.Infrastructure.TableStorage;

namespace ASP.Infrastructure.Azure.TableStorage
{
    public class AzureTableStorageOptions : TableStorageOptions
    {
        public string ManagedIdentityClientId { get; set; } = "";
        public string TableName { get; set; } = "";
        public string StorageAccountName { get; set; } = "";
        public string PrimaryKey { get; set; } = "";
    }
}
