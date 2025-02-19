using ASP.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace ASP.Core.UnitTests;

public class ConfigurationHelperTests
{
    private readonly ConfigurationHelper _helper;

    public ConfigurationHelperTests()
    {
        _helper = new ConfigurationHelper();
    }

    [Fact]
    public void BindConfiguration_WhenConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IConfiguration configuration = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _helper.BindConfiguration<TestConfig>(configuration));
    }

    [Fact]
    public void BindConfiguration_WithEmptyConfiguration_ReturnsNewInstance()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var result = _helper.BindConfiguration<TestConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TestConfig>(result);
    }

    [Fact]
    public void BindConfiguration_WithValuesSection_ReturnsPopulatedInstance()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:TestConfig:StringValue", "test" },
                { "Values:TestConfig:IntValue", "42" }
            }!)
            .Build();

        // Act
        var result = _helper.BindConfiguration<TestConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.StringValue);
        Assert.Equal(42, result.IntValue);
    }

    [Fact]
    public void BindConfiguration_WithRegularSection_ReturnsPopulatedInstance()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "TestConfig:StringValue", "test" },
                { "TestConfig:IntValue", "42" }
            }!)
            .Build();

        // Act
        var result = _helper.BindConfiguration<TestConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.StringValue);
        Assert.Equal(42, result.IntValue);
    }

    [Fact]
    public void ConfigureServices_WhenConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IConfiguration configuration = null!;
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _helper.ConfigureServices<TestConfig>(services, configuration, out _));
    }

    [Fact]
    public void ConfigureServices_WhenServicesIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        IServiceCollection services = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _helper.ConfigureServices<TestConfig>(services, configuration, out _));
    }

    [Fact]
    public void ConfigureServices_WithValuesSection_ConfiguresServices()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:TestConfig:StringValue", "test" },
                { "Values:TestConfig:IntValue", "42" }
            }!)
            .Build();
        var services = new ServiceCollection();

        // Act
        _helper.ConfigureServices<TestConfig>(services, configuration, out var config);

        // Assert
        Assert.NotNull(config);
        Assert.Equal("test", config.StringValue);
        Assert.Equal(42, config.IntValue);

        var serviceProvider = services.BuildServiceProvider();
        var options = ServiceProviderServiceExtensions.GetService<IOptions<TestConfig>>(serviceProvider);
        Assert.NotNull(options);
        Assert.Equal("test", options.Value.StringValue);
        Assert.Equal(42, options.Value.IntValue);
    }

    [Fact]
    public void BindConfiguration_WithContainersList_ReturnsPopulatedInstance()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:DocumentDatabase:DatabaseName", "mydb" },
                { "Values:DocumentDatabase:Containers:0:ContainerName", "container1" },
                { "Values:DocumentDatabase:Containers:0:PartitionKey", "/id" },
                { "Values:DocumentDatabase:Containers:1:ContainerName", "container2" },
                { "Values:DocumentDatabase:Containers:1:PartitionKey", "/type" }
            }!)
            .Build();

        var helper = new ConfigurationHelper();

        // Act
        var result = helper.BindConfiguration<DocumentDatabaseConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("mydb", result.DatabaseName);
        Assert.NotNull(result.Containers);
        Assert.Equal(2, result.Containers.Count);

        Assert.Equal("container1", result.Containers[0].ContainerName);
        Assert.Equal("/id", result.Containers[0].PartitionKey);

        Assert.Equal("container2", result.Containers[1].ContainerName);
        Assert.Equal("/type", result.Containers[1].PartitionKey);
    }

    [Fact]
    public void ConfigureServices_WithContainersList_ConfiguresServices()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:DocumentDatabase:DatabaseName", "mydb" },
                { "Values:DocumentDatabase:Containers:0:ContainerName", "container1" },
                { "Values:DocumentDatabase:Containers:0:PartitionKey", "/id" },
                { "Values:DocumentDatabase:Containers:1:ContainerName", "container2" },
                { "Values:DocumentDatabase:Containers:1:PartitionKey", "/type" }
            }!)
            .Build();

        var services = new ServiceCollection();
        var helper = new ConfigurationHelper();

        // Act
        helper.ConfigureServices<DocumentDatabaseConfig>(services, configuration, out var config);

        // Assert
        Assert.NotNull(config);
        Assert.Equal("mydb", config.DatabaseName);
        Assert.NotNull(config.Containers);
        Assert.Equal(2, config.Containers.Count);

        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<DocumentDatabaseConfig>>();
        Assert.NotNull(options);
        Assert.Equal("mydb", options.Value.DatabaseName);
        Assert.Equal(2, options.Value.Containers.Count);
        Assert.Equal("container1", options.Value.Containers[0].ContainerName);
        Assert.Equal("/id", options.Value.Containers[0].PartitionKey);
        Assert.Equal("container2", options.Value.Containers[1].ContainerName);
        Assert.Equal("/type", options.Value.Containers[1].PartitionKey);
    }

    [Fact]
    public void BindConfiguration_WithEmptyContainersList_ReturnsEmptyList()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:DocumentDatabase:DatabaseName", "mydb" }
            }!)
            .Build();

        var helper = new ConfigurationHelper();

        // Act
        var result = helper.BindConfiguration<DocumentDatabaseConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("mydb", result.DatabaseName);
        Assert.NotNull(result.Containers);
        Assert.Empty(result.Containers);
    }

    [Fact]
    public void BindConfiguration_WithNonSequentialIndexes_BindsAllValues()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Values:DocumentDatabase:DatabaseName", "mydb" },
                { "Values:DocumentDatabase:Containers:0:ContainerName", "container1" },
                { "Values:DocumentDatabase:Containers:2:ContainerName", "container2" }
            }!)
            .Build();

        var helper = new ConfigurationHelper();

        // Act
        var result = helper.BindConfiguration<DocumentDatabaseConfig>(configuration);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("mydb", result.DatabaseName);
        Assert.Equal(2, result.Containers.Count);
        Assert.Equal("container1", result.Containers[0].ContainerName);
        Assert.Equal("container2", result.Containers[1].ContainerName);
    }

    public class DocumentDatabaseConfig
    {
        public const string SectionName = "DocumentDatabase";
        public string DatabaseName { get; set; } = string.Empty;
        public List<ContainerConfig> Containers { get; set; } = new();
    }

    public class ContainerConfig
    {
        public string ContainerName { get; set; } = string.Empty;
        public string PartitionKey { get; set; } = string.Empty;
    }

    private class TestConfig
    {
        public string? StringValue { get; set; } = null;
        public int IntValue { get; set; } = default;
    }
}