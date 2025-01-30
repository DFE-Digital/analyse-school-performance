namespace ASP.Infrastructure.Blob
{
    public class BlobStorageOptions
    {
        public const string SectionName = "BlobStorage";

        public bool InMemory { get; set; } = false;
    }
}
