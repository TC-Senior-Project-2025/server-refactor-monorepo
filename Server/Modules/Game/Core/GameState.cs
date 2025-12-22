namespace Server.Modules.Game.Core;

/// <summary>
/// The state of the game.
/// </summary>
public class GameState
{
    /// <summary>
    /// The ID of the save.
    /// </summary>
    public int? SaveId { get; set; }

    /// <summary>
    /// The current turn.
    /// </summary>
    public required int Turn { get; set; }

    public required string PlayerCountryCode { get; init; }

    public required Dictionary<string, Country> Countries { get; init; }
    
    public GameEvent? CurrentGameEvent { get; set; } = null;
    
    private const int MaxRecentEvents = 3;
    public List<RecentEvent> RecentGameEvents { get; init; } = [];
    
    /// <summary>
    /// Gets the domain-layer player country instance, based on the player's country code.
    /// </summary>
    /// <returns>The domain-layer player country instance.</returns>
    /// <exception cref="InvalidOperationException">The country does not exist inside <c>Countries</c>. This is a game-breaking error.</exception>
    public Country GetPlayerCountry()
    {
        if (!Countries.TryGetValue(PlayerCountryCode, out var country))
            throw new InvalidOperationException(
                $"Player country '{PlayerCountryCode}' not found in game state.");
        return country;
    }
    
    public void PushRecentEvent(RecentEvent e)
    {
        RecentGameEvents.Add(e);
        if (RecentGameEvents.Count > MaxRecentEvents)
            RecentGameEvents.RemoveAt(0);
    }
}