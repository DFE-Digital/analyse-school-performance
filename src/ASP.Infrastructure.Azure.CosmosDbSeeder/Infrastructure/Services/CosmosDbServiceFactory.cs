using ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;

public class CosmosDbServiceFactory : ICosmosDbServiceFactory
{
    private readonly IOptions<SourceCosmosDbOptions> _sourceOptions;
    private readonly IOptions<TargetCosmosDbOptions> _targetOptions;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IJsonDocumentProcessor _documentProcessor;

    public CosmosDbServiceFactory(
        IOptions<SourceCosmosDbOptions> sourceOptions,
        IOptions<TargetCosmosDbOptions> targetOptions,
        ILoggerFactory loggerFactory,
        IJsonDocumentProcessor documentProcessor)
    {
        _sourceOptions = sourceOptions ?? throw new ArgumentNullException(nameof(sourceOptions));
        _targetOptions = targetOptions ?? throw new ArgumentNullException(nameof(targetOptions));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _documentProcessor = documentProcessor ?? throw new ArgumentNullException(nameof(documentProcessor));
    }

    public ICosmosDbService CreateSourceService()
    {
        var options = _sourceOptions.Value;
        options.Validate(nameof(SourceCosmosDbOptions));

        return new CosmosDbService(
            Options.Create(options),
            _loggerFactory.CreateLogger<CosmosDbService>(),
            _documentProcessor);
    }

    public ICosmosDbService CreateTargetService()
    {
        var options = _targetOptions.Value;
        options.Validate(nameof(TargetCosmosDbOptions));

        return new CosmosDbService(
            Options.Create(options),
            _loggerFactory.CreateLogger<CosmosDbService>(),
            _documentProcessor);
    }
}