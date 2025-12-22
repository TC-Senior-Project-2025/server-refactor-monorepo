using Microsoft.Extensions.Options;

namespace Server.Common.Config;

/// <summary>
/// Service for accessing application configuration.
/// </summary>
public class ConfigService(IOptions<AppConfig> appConfig, IConfiguration configuration)
{
    private readonly AppConfig _appConfig = appConfig.Value;

    /// <summary>
    /// Gets the message of the day from configuration.
    /// </summary>
    public string Motd => _appConfig.Motd;
    /// <summary>
    /// Gets whether features are enabled from configuration.
    /// </summary>
    public bool FeatureEnabled => _appConfig.FeatureEnabled;

    /// <summary>
    /// Gets a configuration value by key.
    /// </summary>
    /// <param name="key">The configuration key.</param>
    /// <returns>The configuration value, or empty string if not found.</returns>
    public string Get(string key)
    {
        return configuration[key] ?? string.Empty;
    }
}
