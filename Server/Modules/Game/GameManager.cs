using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json;
using Server.Common.WebSockets;
using Server.Modules.Assets;
using Server.Modules.Game.Actions;
using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;
using Server.Modules.Game.Dto;
using Server.Modules.Game.Events;
using Server.Modules.Game.Memory;
using Server.Modules.Game.Services;
using Server.Modules.Game.Sessions;
using Server.Modules.SaveGames;

namespace Server.Modules.Game;

/// <summary>
/// Manages game-related WebSocket actions and operations.
/// </summary>
public class GameManager(
    ILogger<GameManager> logger,
    IGameSessionStore sessionStore,
    SaveGamesService saveGamesService,
    EventGenerator eventGenerator,
    AssetsService assetsService,
    HistorySummarizer historySummarizer,
    RecentSituationSummarizer recentSituationSummarizer,
    IEnumerable<IPlayerActionHandler> playerActionHandlers
    )
{

    private readonly IGameSessionStore _sessionStore = sessionStore;
    private readonly Dictionary<PlayerActionType, IPlayerActionHandler> _playerActionHandlers =
        playerActionHandlers.ToDictionary(handler => handler.Type);

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

        var mapConnections = assetsService.LoadMapConnections();
        if (mapConnections == null)
        {
            await SendError(context.Socket, "Failed to load map connections");
            return;
        }

        await context.Socket.SendTopic("C_StartGame", new StartGameResponse
        {
            MapConnections = mapConnections.AsDict(),
            GameState = gameState
        });
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

        var country = gameState.GetPlayerCountry();
        if (country == null)
        {
            logger.LogError("Player country not found");
            await SendError(context.Socket, "Player country not found");
            return;
        }

        // Process turn
        var turnProcessResult = TurnService.ProcessTurn(gameState);

        gameState.CurrentPhase = GamePhase.EventGeneration;
        gameState.Turn++;

        await context.Socket.SendTopic("C_DisplayTurnProcessResult", turnProcessResult);
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

        if (!_playerActionHandlers.TryGetValue(playerAction.Action, out var handler))
        {
            logger.LogError("No handler registered for action {Action}", playerAction.Action);
            await SendError(context.Socket, "Invalid player action");
            return;
        }

        var payloadType = handler.PayloadType ?? handler.GetType().GetCustomAttribute<PlayerActionHandlerAttribute>()?.PayloadType;
        object? typedPayload = null;

        if (payloadType != null)
        {
            try
            {
                typedPayload = playerAction.ParsePayload(payloadType);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to parse payload for action {Action}", playerAction.Action);
                await SendError(context.Socket, "Failed to parse payload");
                return;
            }
        }

        try
        {
            await handler.HandleAsync(context, gameState, typedPayload);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error handling player action {Action}", playerAction.Action);
            await SendError(context.Socket, e.Message);
            return;
        }

        gameState.CurrentPhase = GamePhase.Start;
        await context.Socket.SendTopic("C_UpdateGameState", gameState);
        await context.Socket.SendTopic("C_NewTurn");
    }
}
