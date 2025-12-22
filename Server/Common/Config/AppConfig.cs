namespace Server.Common.Config;

/// <summary>
/// Configuration class for application settings.
/// </summary>
public class AppConfig
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "App";

    /// <summary>
    /// Gets or sets the message of the day.
    /// </summary>
    public string Motd { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets whether certain features are enabled.
    /// </summary>
    public bool FeatureEnabled { get; set; }
}
