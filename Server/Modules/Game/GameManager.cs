using System.Net.WebSockets;
using System.Text.Json;
using Server.Common.WebSockets;
using Server.Modules.Countries;
using Server.Modules.Game.Actions;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
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
        // TODO: Use DTO
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

        var currentSession = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (currentSession != null)
        {
            await _sessionStore.Remove(currentSession.Id);
        }
        
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
        
        var gameState = session.GameState;
        if (gameState.CurrentPhase != GamePhase.Start)
        {
            await SendError(context.Socket, "Game not in start phase");
            return;
        }

        gameState.CurrentPhase = GamePhase.EventGeneration;
        
        logger.LogInformation("Advancing turn...");
        gameState.Turn++;
        
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
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
        if (gameState.CurrentPhase != GamePhase.EventGeneration)
        {
            await SendError(context.Socket, "Game not in event generation phase");
            return;
        }
        
        logger.LogInformation("Generating event...");
        // var gameEvent = await eventGenerator.GenerateEventAsync(gameState);
        var gameEvent = await eventGenerator.GenerateEventExampleAsync();
        
        gameState.CurrentGameEvent = gameEvent;
        gameState.CurrentPhase = GamePhase.EventOptionSelect;
        gameState.GetPlayerCountry().Resources.ApplyChanges(gameEvent.ResourceChanges);
        
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
        // var effects = await eventGenerator.GenerateEventOptionEffectsAsync(option);
        var effects = await eventGenerator.GenerateEventOptionEffectsExampleAsync();

        var recentEvent = new RecentEvent
        {
            Title = currentGameEvent.Title,
            Description = currentGameEvent.Description,
            ChosenOptionTitle = option.Title,
            ChosenOptionDescription = option.Description
        };
        
        // TODO: Introduce a bounded list so that we can derive recent events without mutating (prevent lost changes)
        
        // Push the recent event
        gameState.PushRecentEvent(recentEvent);
        
        // Generate recent situation summary
        var recentSituationSummary = "test";
        // var recentSituationSummary = await recentSituationSummarizer.SummarizeAsync(
        //     gameState.PlayerCountryCode, 
        //     gameState.Turn, 
        //     gameState.GetPlayerCountry().RecentSituationSummary,
        //     gameState.RecentGameEvents
        // );
        
        // Consume the current event
        gameState.CurrentGameEvent = null;
        
        // Update player country resources
        gameState.GetPlayerCountry().Resources.ApplyChanges(effects.ResourceChanges);

        // Update player country summary
        gameState.GetPlayerCountry().RecentSituationSummary = recentSituationSummary;
        
        // Transition to player action phase
        gameState.CurrentPhase = GamePhase.PlayerAction;
        
        // Save the game state
        // if (gameState.SaveId == null)
        // {
        //     logger.LogWarning("Save game ID not found for game state");
        //     return;
        // }
        //
        // logger.LogInformation("Saving game state...");
        // await saveGamesService.UpdateSaveGame(gameState.SaveId.Value, gameState);
        
        await context.Socket.SendTopic("C_DisplayEventOptionEffects", effects);
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
    }

    [GameAction("S_SaveGame")]
    public async Task SaveGame(GameActionContext context)
    {
        var session = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (session == null)
        {
            await SendError(context.Socket, "Session not found for user");
            return;
        }
        
        var gameState = session.GameState;
        if (gameState.SaveId == null)
        {
            logger.LogWarning("Save game ID not found for game state");
            return;
        }
        
        logger.LogInformation("Saving game state...");
        await saveGamesService.UpdateSaveGame(gameState.SaveId.Value, gameState);
        
        await context.Socket.SendTopic("C_SaveGameResponse", "Game saved successfully");
    }

    [GameAction("S_PlayerAction")]
    public async Task HandlePlayerAction(GameActionContext context)
    {
        PlayerActionDto? playerActionDto;
        try
        {
            playerActionDto = context.Payload.Deserialize<PlayerActionDto>();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error parsing player action");
            await SendError(context.Socket, "Error parsing player action");
            return;
        }

        if (playerActionDto == null)
        {
            logger.LogError("Player action not found");
            await SendError(context.Socket, "Player action not found");
            return;
        }
        
        var session = await _sessionStore.GetByUserId(context.UserEntity.Id);
        if (session == null)
        {
            await SendError(context.Socket, "Session not found for user");
            return;
        }

        var gameState = session.GameState;
        
        if (gameState.CurrentPhase != GamePhase.PlayerAction)
        {
            await SendError(context.Socket, "Game not in player action phase");
            return;
        }

        var playerAction = PlayerAction.From(playerActionDto);
        if (playerAction == null)
        {
            logger.LogError("Invalid player action");
            await SendError(context.Socket, "Invalid player action");
            return;       
        }
        
        switch (playerAction.Action)
        {
            case PlayerActionType.IncreaseMilitaryBudget:
                gameState.GetPlayerCountry().Resources.Treasury -= 10;
                gameState.GetPlayerCountry().Resources.Manpower += 20;
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown player action: {playerAction.Action.ToString()}");
        }
        
        gameState.CurrentPhase = GamePhase.Start;
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
        await context.Socket.SendTopic("C_NewTurn");
    }
}
