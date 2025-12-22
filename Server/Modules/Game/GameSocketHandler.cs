using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Server.Common.WebSockets;
using Server.Modules.Auth;
using Server.Modules.Users;
using Server.Modules.Users.Entities;

namespace Server.Modules.Game;

/// <summary>
/// Handles WebSocket connections for game-related real-time communication.
/// </summary>
public class GameSocketHandler
{
    private readonly ILogger<GameSocketHandler> _logger;
    private readonly GameManager _gameManager;
    private readonly WebSocketConnectionManager _connectionManager;
    private readonly AuthService _authService;
    private readonly Dictionary<string, MethodInfo> _handlers = [];

    /// <summary>
    /// Initializes a new instance of the GameSocketHandler and scans for game actions.
    /// </summary>
    /// <param name="logger">Logger for diagnostic information.</param>
    /// <param name="gameManager">The game manager containing action handlers.</param>
    /// <param name="connectionManager">The WebSocket connection manager.</param>
    /// <param name="authService">The authentication service.</param>
    public GameSocketHandler(ILogger<GameSocketHandler> logger, GameManager gameManager, WebSocketConnectionManager connectionManager, AuthService authService)
    {
        _logger = logger;
        _gameManager = gameManager;
        _connectionManager = connectionManager;
        _authService = authService;

        // Scan GameManager for methods with [GameAction]
        var methods = _gameManager.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<GameActionAttribute>();
            if (attr != null)
            {
                _handlers[attr.Topic] = method;
            }
        }
    }

    /// <summary>
    /// Handles an incoming WebSocket connection request.
    /// </summary>
    /// <param name="context">The HTTP context containing the WebSocket request.</param>
    public async Task HandleAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketId = _connectionManager.AddSocket(webSocket);
            if (webSocketId == null)
            {
                throw new Exception("Failed to add socket to connection manager");
            }

            _logger.LogInformation("WebSocket connected.");

            var tokenString = context.Request.Query["token"].ToString();
            var tokenGuid = new Guid(tokenString);
            var user = await _authService.ValidateToken(tokenGuid);

            if (user == null)
            {
                throw new Exception("Invalid token");
            }

            try
            {
                await ProcessLoop(webSocket, user);
                await _connectionManager.RemoveSocketAsync(webSocketId);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "WebSocket error");
            }
            finally
            {
                _logger.LogInformation("WebSocket disconnected.");
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task ProcessLoop(WebSocket webSocket, UserEntity userEntity)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var content = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation("Received message: {Content}", content);

                    try
                    {
                        var message = JsonSerializer.Deserialize<WebSocketMessage>(content);

                        if (message != null && _handlers.TryGetValue(message.Topic, out var handler))
                        {
                            // Create a new context to be passed to the method (HIGHLY SCALABLE :D)
                            var context = new GameActionContext
                            {
                                Payload = message.Payload,
                                Socket = webSocket,
                                UserEntity = userEntity
                            };
                            
                            // Invoke method on _gameManager instance
                            // We need to await the task to ensure exceptions are caught here
                            if (handler.Invoke(_gameManager, [context]) is Task task)
                            {
                                await task;
                            }
                        }
                        else
                        {
                            _logger.LogWarning("No handler found for topic: {Topic}", message?.Topic);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing WebSocket message logic.");
                    }
                }
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning("WebSocket connection error: {Message}", ex.Message);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in WebSocket loop.");
                if (webSocket.State != WebSocketState.Open) break;
            }
        }
    }
}
