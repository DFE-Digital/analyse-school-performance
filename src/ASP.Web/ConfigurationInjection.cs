namespace ASP.Web;

/// <summary>
/// Workaround for issue where WebApplicationFactory&lt;TProgram&gt; only allows overriding/adding configuration sources after
/// the service collection has already been built. This means any test code that adds configuration sources would have to 
/// to rebuild the service collection in order to make use of the updated configuration.
/// This workaround creates an injection point in Program.cs where configuration sources can be added/overridden before the 
/// service collection is built, so the existing dependency injection code in Program.cs can be driven by the updated 
/// configuration. 
/// Based on workaround in this issue: https://github.com/dotnet/aspnetcore/issues/37680#issuecomment-1331559463
/// </summary>
public static class ConfigurationInjection
{
    // This async local is set in from tests and it flows to main
    public static readonly AsyncLocal<Action<IConfigurationBuilder>?> _current = new();

    /// <summary>
    /// Adds the current test configuration to the application in the "right" place
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder</param>
    /// <returns>The modified <see cref="IConfigurationBuilder"/></returns>
    public static IConfigurationBuilder AddInjectedConfiguration(this IConfigurationBuilder configurationBuilder)
    {
        if (_current.Value is { } configure)
        {
            configure(configurationBuilder);
        }

        return configurationBuilder;
    }

    /// <summary>
    /// Unit tests can use this to flow state to the main program and change configuration
    /// </summary>
    /// <param name="action"></param>
    public static void Inject(Action<IConfigurationBuilder> action)
    {
        _current.Value = action;
    }
}