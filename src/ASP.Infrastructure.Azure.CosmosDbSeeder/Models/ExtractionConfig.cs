namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Models;

public class ExtractionConfig
{
    public bool Enabled { get; set; }
    public bool CleanupBeforeExtract { get; set; } = false; 
    public List<string> ExcludeProperties { get; set; } = new(); 
}