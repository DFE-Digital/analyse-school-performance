using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Core.Configuration;

public class ConfigurationHelper
{
   public T BindConfiguration<T>(IConfiguration configuration) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var (boundConfig, _) = BindConfigurationInternal<T>(configuration, out var defaultConfig);
        return boundConfig ?? defaultConfig;
    }

    public void ConfigureServices<T>(
        IServiceCollection services,
        IConfiguration configuration,
        out T config) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(services);

        config = new T();
        var (boundConfig, configBuilder) = BindConfigurationInternal<T>(configuration, out config);

        if (boundConfig != null)
        {
            config = boundConfig;
            if (configBuilder != null)
            {
                // Configuration from Values section
                services.Configure<T>(options => configBuilder.Bind(options));
            }
            else
            {
                // Configuration from regular section
                var sectionName = GetSectionName<T>();
                services.Configure<T>(configuration.GetSection(sectionName));
            }
        }
        else
        {
            // If no configuration is found, configure with empty options
            services.Configure<T>(options => { });
        }
    }

    private (T? Config, IConfiguration? Builder) BindConfigurationInternal<T>(
        IConfiguration configuration, 
        out T defaultConfig) where T : class, new()
    {
        var sectionName = GetSectionName<T>();
        defaultConfig = new T();

        // Check Values section first (Azure Functions)
        var valuesSection = configuration.GetSection("Values").GetSection(sectionName);
        if (HasChildren(valuesSection))
        {
            var flattenedValues = GetFlattenedValues(valuesSection);
            if (flattenedValues.Any())
            {
                var configBuilder = new ConfigurationBuilder()
                    .AddInMemoryCollection(flattenedValues.Select(kvp =>
                        new KeyValuePair<string, string?>(kvp.Key, kvp.Value)))
                    .Build();

                configBuilder.Bind(defaultConfig);
                return (defaultConfig, configBuilder);
            }
        }

        // If Values section doesn't exist or is empty, try regular configuration section
        var regularSection = configuration.GetSection(sectionName);
        if (HasChildren(regularSection))
        {
            return (regularSection.Get<T>(), null);
        }

        return (null, null);
    }

    private static string GetSectionName<T>()
    {
        var sectionNameField = typeof(T).GetField("SectionName",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return sectionNameField?.GetRawConstantValue() as string ?? typeof(T).Name;
    }

    private static bool HasChildren(IConfigurationSection section)
    {
        return section.GetChildren().Any();
    }

    private static IDictionary<string, string> GetFlattenedValues(IConfigurationSection section)
    {
        var data = new Dictionary<string, string>();
        FlattenSection(section, "", data);
        return data;
    }

    private static void FlattenSection(IConfigurationSection section, string prefix,
        IDictionary<string, string> data)
    {
        foreach (var child in section.GetChildren())
        {
            var key = string.IsNullOrEmpty(prefix) ? child.Key : $"{prefix}:{child.Key}";

            if (child.Value != null)
            {
                data[key] = child.Value;
            }
            else
            {
                FlattenSection(child, key, data);
            }
        }
    }
}