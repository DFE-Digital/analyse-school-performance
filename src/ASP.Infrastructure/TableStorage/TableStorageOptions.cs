namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageOptions
    {
        public const string SectionName = "TableStorage";

        public string ManagedIdentityClientId { get; set; } = "";
        public string TableName { get; set; } = "";
        public string StorageAccountName { get; set; } = "";
        public string PrimaryKey { get; set; } = "";
    }
}
