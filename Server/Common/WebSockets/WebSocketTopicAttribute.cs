namespace Server.Common.WebSockets;

/// <summary>
/// Attribute to mark methods as WebSocket topic handlers.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class WebSocketTopicAttribute(string topic) : Attribute
{
    /// <summary>
    /// Gets the topic this handler is associated with.
    /// </summary>
    public string Topic { get; } = topic;
}
