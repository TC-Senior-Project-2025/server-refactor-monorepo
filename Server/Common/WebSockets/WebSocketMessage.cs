using System.Text.Json;

namespace Server.Common.WebSockets;

/// <summary>
/// Represents a WebSocket message with a topic and payload.
/// </summary>
public class WebSocketMessage
{
    /// <summary>
    /// Gets or sets the message topic.
    /// </summary>
    public string Topic { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the message payload as a JSON element.
    /// </summary>
    public JsonElement Payload { get; set; }
}
