using System.Collections.Concurrent;

namespace Server.Modules.Game.Sessions;

/// <summary>
/// In-memory implementation of <see cref="IGameSessionStore"/>.
/// </summary>
public class InMemoryGameSessionStore : IGameSessionStore
{
    private readonly ConcurrentDictionary<Guid, GameSession> _sessions = new();
    private readonly ConcurrentDictionary<int, Guid> _userIdToSessionId = new();

    /// <inheritdoc />
    public Task Add(GameSession session)
    {
        _sessions.TryAdd(session.Id, session);
        _userIdToSessionId.TryAdd(session.UserId, session.Id);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<GameSession?> Get(Guid id)
    {
        _sessions.TryGetValue(id, out var session);
        return Task.FromResult(session);
    }

    /// <inheritdoc/>
    public Task<GameSession?> GetByUserId(int userId)
    {
        _userIdToSessionId.TryGetValue(userId, out var sessionId);
        _sessions.TryGetValue(sessionId, out var session);
        return Task.FromResult(session);
    }

    /// <inheritdoc />
    public Task Remove(Guid id)
    {
        _sessions.TryRemove(id, out var session);
        if (session == null) return Task.CompletedTask;
        
        _userIdToSessionId.TryRemove(session.UserId, out _);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IEnumerable<GameSession>> GetAll()
    {
        return Task.FromResult((IEnumerable<GameSession>)_sessions.Values);
    }
}
