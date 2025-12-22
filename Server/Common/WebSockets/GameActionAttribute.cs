namespace Server.Common.WebSockets;

/// <summary>
/// Attribute to mark methods as game action handlers for WebSocket messages.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class GameActionAttribute(string topic) : Attribute
{
    /// <summary>
    /// Gets the topic this action handles.
    /// </summary>
    public string Topic { get; } = topic;
}
