namespace Server.Common.Config;

/// <summary>
/// Module for registering configuration services.
/// </summary>
public static class ConfigModule
{
    /// <summary>
    /// Registers configuration services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfig>(configuration.GetSection(AppConfig.SectionName));
        services.AddSingleton<ConfigService>();
    }
}
