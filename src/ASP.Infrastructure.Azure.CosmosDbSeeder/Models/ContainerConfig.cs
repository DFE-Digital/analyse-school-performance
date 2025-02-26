namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Models;

public class ContainerConfig
{
    public ContainerConfig(string name, string partitionKeyPath, int? throughput, string dataPath, string whereClause)
    {
        Name = name;
        PartitionKeyPath = partitionKeyPath;
        Throughput = throughput;
        DataPath = dataPath;
        WhereClause = whereClause;
    }

    public string Name { get; set; }
    public string PartitionKeyPath { get; set; }
    public int? Throughput { get; set; }
    public string DataPath { get; set; }
    public string WhereClause { get; set; }
}