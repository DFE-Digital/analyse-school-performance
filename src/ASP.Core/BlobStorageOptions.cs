namespace ASP.Core
{
    public class BlobStorageOptions
    {
        public const string SectionName = "BlobStorage";

        public bool InMemory { get; set; } = false;
    }
}
