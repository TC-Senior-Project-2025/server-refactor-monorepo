using Server.Modules.Game.Core;

namespace Server.Modules.Game.Sessions;

/// <summary>
/// Represents an active game session.
/// </summary>
public class GameSession
{
    /// <summary>
    /// Gets or sets the unique identifier for the session.
    /// </summary>
    public required Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Gets or sets the unique identifier for the user associated with the session.
    /// </summary>
    public required int UserId { get; set; }

    /// <summary>
    /// Gets or sets the current game state for the session.
    /// </summary>
    public required GameState GameState { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the session was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the session was last active.
    /// </summary>
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
}
