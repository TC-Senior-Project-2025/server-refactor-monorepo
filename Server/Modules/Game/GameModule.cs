using Server.Common.Llm.Interfaces;
using Server.Common.Llm.Services;
using Server.Common.WebSockets;
using Server.Modules.Game.Events;
using Server.Modules.Game.Memory;
using Server.Modules.Game.Sessions;
using Server.Modules.SaveGames;

namespace Server.Modules.Game;

/// <summary>
/// Module for registering game-related services and WebSocket endpoints.
/// </summary>
public static class GameModule
{
    /// <summary>
    /// Registers game services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IGameSessionStore, InMemoryGameSessionStore>();
        services.AddSingleton<WebSocketConnectionManager>();
        services.AddScoped<GameSocketHandler>();
        services.AddScoped<GameManager>();
        services.AddScoped<EventGenerator>();
        services.AddScoped<HistorySummarizer>();
    }

    /// <summary>
    /// Maps game-related WebSocket endpoints.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.Map("/game", async (HttpContext context) =>
        {
            var handler = context.RequestServices.GetRequiredService<GameSocketHandler>();
            await handler.HandleAsync(context);
        }).RequireAuthorization();
    }
}
