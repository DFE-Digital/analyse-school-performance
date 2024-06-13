namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? ManagedIdentityClientId { get; set; }
        public string? TableName { get; set; }
        public string? StorageAccountName { get; set; }
    }
}
