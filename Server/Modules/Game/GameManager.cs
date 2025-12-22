using System.Net.WebSockets;
using System.Text.Json;
using Server.Common.WebSockets;
using Server.Modules.Countries;
using Server.Modules.Game.Sessions;
using Server.Modules.SaveGames;
using Server.Modules.Game.Core;
using Server.Modules.Game.Events;
using Server.Modules.Game.Memory;

namespace Server.Modules.Game;

/// <summary>
/// Manages game-related WebSocket actions and operations.
/// </summary>
public class GameManager(
    ILogger<GameManager> logger,
    IGameSessionStore sessionStore,
    SaveGamesService saveGamesService, 
    EventGenerator eventGenerator,
    CountriesService countriesService,
    HistorySummarizer historySummarizer,
    RecentSituationSummarizer recentSituationSummarizer
    )
{
    private readonly IGameSessionStore _sessionStore = sessionStore;

    /// <summary>
    /// Sends an error message to a connected WebSocket client.
    /// </summary>
    /// <param name="socket">The WebSocket instance representing the client's connection.</param>
    /// <param name="message">The error message to be sent to the client.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of sending the error message.</returns>
    private async Task SendError(WebSocket socket, string message)
    {
        logger.LogWarning("Handled error: {}", message);
        await socket.SendTopic("C_DisplayError", message);
    }
    
    /// <summary>
    /// Responds with a hello message.
    /// </summary>
    [GameAction("S_Hello")]
    public async Task Hello(GameActionContext context)
    {
        logger.LogInformation("Saying hello...");
        await context.Socket.SendTopic("C_HelloResponse", $"Hi, {context.UserEntity.Username}!");
    }

    /// <summary>
    /// Starts a game from a saved game.
    /// </summary>
    [GameAction("S_StartFromSavedGame")]
    public async Task StartFromSavedGame(GameActionContext context)
    {
        logger.LogInformation("Starting from saved game...");

        // TODO: Handle get JSON property error
        var saveGameId = context.Payload.GetProperty("SaveGameId").GetInt32();
        var saveGame = await saveGamesService.GetSaveGame(saveGameId);

        if (saveGame == null)
        {
            await SendError(context.Socket, "Save game not found");
            return;
        }

        var gameState = JsonSerializer.Deserialize<GameState>(saveGame.GameStateJson);
        if (gameState == null)
        {
            await SendError(context.Socket, "Game state not found");
            return;
        }
        
        gameState.SaveId = saveGameId;
        
        var session = new GameSession
        {
            Id = Guid.NewGuid(),
            UserId = context.UserEntity.Id,
            GameState = gameState,
            CreatedAt = default,
            LastActiveAt = default
        };
        
        await _sessionStore.Add(session);
        logger.LogInformation("Created session {SessionId}", session.Id);  
        
        await context.Socket.SendTopic("C_StartGame", gameState);
    }

    [GameAction("S_NewTurn")]
    public async Task NewTurn(GameActionContext context)
    {
        var session = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (session == null)
        {
            await SendError(context.Socket, "Session not found for user");
            return;
        }
    }

    /// <summary>
    /// Handles the generation and dispatch of an in-game event for the user's active game session.
    /// </summary>
    /// <param name="context">The context containing information about the WebSocket connection, user, and payload.</param>
    /// <return>Returns a Task representing the asynchronous operation. Sends the generated event to the specified WebSocket topic.</return>
    [GameAction("S_GenerateEvent")]
    public async Task GenerateEvent(GameActionContext context)
    {
        var session = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (session == null)
        {
            await SendError(context.Socket, "Session not found for user");
            return;
        }

        var gameState = session.GameState;
        
        logger.LogInformation("Generating event...");
        var gameEvent = await eventGenerator.GenerateEventAsync(gameState);
        gameState.CurrentGameEvent = gameEvent;

        await context.Socket.SendTopic("C_DisplayEvent", gameEvent);
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
    }
    
    /// <summary>
    /// Handles the generation and dispatch of an in-game event for the user's active game session.
    /// </summary>
    /// <param name="context">The context containing information about the WebSocket connection, user, and payload.</param>
    /// <return>Returns a Task representing the asynchronous operation. Sends the generated event to the specified WebSocket topic.</return>
    [GameAction("S_GenerateEventOptionEffects")]
    public async Task GenerateEventOptionEffects(GameActionContext context)
    {
        // TODO: Handle get JSON property error
        var optionIndex = context.Payload.GetProperty("OptionIndex").GetInt32();
   
        var session = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (session == null)
        {
            await SendError(context.Socket, "Session not found for user");
            return;
        }

        var gameState = session.GameState;
        var currentGameEvent = gameState.CurrentGameEvent;
        if (currentGameEvent == null)
        {
            await SendError(context.Socket, "No current game event");
            return;
        }

        if (optionIndex >= currentGameEvent.Options.Count || optionIndex < 0)
        {
            await SendError(context.Socket, "Option index out of range");
            return;
        }
        
        var option = currentGameEvent.Options[optionIndex];
        
        logger.LogInformation("Generating option effects...");
        var effects = await eventGenerator.GenerateEventOptionEffects(option);

        var recentEvent = new RecentEvent
        {
            Title = currentGameEvent.Title,
            Description = currentGameEvent.Description,
            ChosenOptionTitle = option.Title,
            ChosenOptionDescription = option.Description
        };;
        
        // Push the recent event
        gameState.PushRecentEvent(recentEvent);
        
        // Consume the current event
        gameState.CurrentGameEvent = null;
        
        // Generate recent situation summary
        var recentSituationSummary = await recentSituationSummarizer.SummarizeAsync(
            gameState.PlayerCountryCode, 
            gameState.Turn, 
            gameState.GetPlayerCountry().RecentSituationSummary,
            gameState.RecentGameEvents
        );
        
        // Update player country summary
        gameState.GetPlayerCountry().RecentSituationSummary = recentSituationSummary;
        
        // Save the game state
        if (gameState.SaveId == null)
        {
            logger.LogWarning("Save game ID not found for game state");
            return;
        }
        
        logger.LogInformation("Saving game state...");
        await saveGamesService.UpdateSaveGame(gameState.SaveId.Value, gameState);
        
        await context.Socket.SendTopic("C_DisplayEventOptionEffects", effects);
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
    }
}
