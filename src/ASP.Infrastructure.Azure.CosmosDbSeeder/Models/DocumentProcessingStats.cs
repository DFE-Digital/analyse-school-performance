namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Models;

public record DocumentProcessingStats
{
    public int Created { get; init; }
    public int Skipped { get; init; }

    public static DocumentProcessingStats Empty => new();

    public DocumentProcessingStats AddCreated() =>
        this with { Created = Created + 1 };

    public DocumentProcessingStats AddSkipped() =>
        this with { Skipped = Skipped + 1 };
}