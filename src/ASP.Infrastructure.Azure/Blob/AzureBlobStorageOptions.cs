using ASP.Core;

namespace ASP.Infrastructure.Azure.Blob
{
    public class AzureBlobStorageOptions : BlobStorageOptions
    {
        public string ManagedIdentityClientId { get; set; } = "";
        public string StorageAccountName { get; set; } = "";
        public string PrimaryKey { get; set; } = "";
    }
}
