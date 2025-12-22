using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Common.WebSockets;

/// <summary>
/// Provides extension methods for working with <see cref="WebSocket"/> instances.
/// </summary>
public static class WebSocketExtensions
{
    /// <summary>
    /// Sends a topic and its associated payload to a <see cref="WebSocket"/> client as a JSON-encoded message.
    /// </summary>
    /// <typeparam name="T">The type of the payload being sent with the topic.</typeparam>
    /// <param name="webSocket">The <see cref="WebSocket"/> instance to send the message to.</param>
    /// <param name="topic">The string representing the topic of the message.</param>
    /// <param name="payload">The payload associated with the topic, which will be serialized to JSON.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous send operation.</returns>
    public static async Task SendTopic<T>(this WebSocket webSocket, string topic, T payload)
    {
        var response = new { Topic = topic, Payload = payload };
        var json = JsonSerializer.Serialize(response);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        await webSocket.SendAsync(
            new ArraySegment<byte>(bytes),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }
}