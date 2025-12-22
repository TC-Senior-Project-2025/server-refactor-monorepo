namespace Server.Modules.SaveGames;

/// <summary>
/// Module for registering save game services.
/// </summary>
public static class SaveGamesModule
{
    /// <summary>
    /// Registers save game services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<SaveGamesService>();
    }
}
