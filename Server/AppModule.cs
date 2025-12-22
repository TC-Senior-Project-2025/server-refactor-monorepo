using Server.Common.Config;
using Server.Common.Database;
using Server.Common.Llm;
using Server.Modules.Auth;
using Server.Modules.Countries;
using Server.Modules.Game;
using Server.Modules.SaveGames;
using Server.Modules.Users;

namespace Server;

/// <summary>
/// Root application module that coordinates registration of all sub-modules and services.
/// </summary>
public static class AppModule
{
    /// <summary>
    /// Registers all application services and modules with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        // Global Configuration
        ConfigModule.Register(services, configuration);
        DatabaseModule.Register(services, configuration);
        LlmModule.Register(services, configuration);

        // Register Sub-Modules
        UsersModule.Register(services);
        GameModule.Register(services);
        AuthModule.Register(services);
        SaveGamesModule.Register(services);
        CountriesModule.Register(services);

        // Framework Services
        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.PropertyNamingPolicy = null;
            o.JsonSerializerOptions.DictionaryKeyPolicy = null;
        });
    }

    /// <summary>
    /// Maps all application endpoints including WebSocket routes.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        GameModule.MapEndpoints(app);
    }
}
