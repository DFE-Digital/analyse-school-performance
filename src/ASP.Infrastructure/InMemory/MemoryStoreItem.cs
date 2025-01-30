namespace ASP.Infrastructure.InMemory
{
    public record MemoryStoreItem<TKey>(TKey Key, string Contents) 
        where TKey : notnull;
}
