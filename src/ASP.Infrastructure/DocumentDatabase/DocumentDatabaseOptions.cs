namespace ASP.Infrastructure.DocumentDatabase
{
    public class DocumentDatabaseOptions {
        public const string SectionName = "DocumentDatabase";

        public bool InMemory { get; set; } = false;
    }
}
