using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Server.Common.WebSockets;

/// <summary>
/// Manages WebSocket connections.
/// </summary>
public class WebSocketConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    /// <summary>
    /// Adds a WebSocket connection to the manager.
    /// </summary>
    /// <param name="socket">The WebSocket connection.</param>
    /// <returns>The unique identifier for the connection.</returns>
    public string AddSocket(WebSocket socket)
    {
        var id = Guid.NewGuid().ToString();
        _sockets.TryAdd(id, socket);
        return id;
    }

    /// <summary>
    /// Gets a WebSocket connection by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the connection.</param>
    /// <returns>The WebSocket connection, or null if not found.</returns>
    public WebSocket? GetSocket(string id)
    {
        return _sockets.TryGetValue(id, out var socket) ? socket : null;
    }

    /// <summary>
    /// Gets all WebSocket connections.
    /// </summary>
    /// <returns>An enumerable of all WebSocket connections.</returns>
    public IEnumerable<KeyValuePair<string, WebSocket>> GetAll()
    {
        return _sockets;
    }

    /// <summary>
    /// Removes a WebSocket connection from the manager.
    /// </summary>
    /// <param name="id">The identifier of the connection.</param>
    public async Task RemoveSocketAsync(string id)
    {
        if (_sockets.TryRemove(id, out var socket))
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
    }

    /// <summary>
    /// Sends a message to a specific WebSocket connection.
    /// </summary>
    /// <param name="socket">The WebSocket connection.</param>
    /// <param name="message">The message to send.</param>
    public async Task SendMessageAsync(WebSocket socket, string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    /// <summary>
    /// Broadcasts a message to all WebSocket connections.
    /// </summary>
    /// <param name="message">The message to broadcast.</param>
    public async Task BroadcastAsync(string message)
    {
        foreach (var pair in _sockets) await SendMessageAsync(pair.Value, message);
    }
}