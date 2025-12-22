namespace Server.Modules.Game.Sessions;

/// <summary>
/// Interface for storing and retrieving game sessions.
/// </summary>
public interface IGameSessionStore
{
    /// <summary>
    /// Adds a new game session to the store.
    /// </summary>
    /// <param name="session">The session to add.</param>
    Task Add(GameSession session);

    /// <summary>
    /// Retrieves a game session by its ID.
    /// </summary>
    /// <param name="id">The session ID.</param>
    /// <returns>The game session, or null if not found.</returns>
    Task<GameSession?> Get(Guid id);

    /// <summary>
    /// Retrieves a game session by its user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns></returns>
    Task<GameSession?> GetByUserId(int userId);
    
    /// <summary>
    /// Removes a game session from the store.
    /// </summary>
    /// <param name="id">The session ID to remove.</param>
    Task Remove(Guid id);

    /// <summary>
    /// Retrieves all active game sessions.
    /// </summary>
    /// <returns>A collection of all game sessions.</returns>
    Task<IEnumerable<GameSession>> GetAll();
}
