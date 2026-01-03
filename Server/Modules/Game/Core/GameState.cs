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

    public required int PlayerCountryId { get; init; }

    public required Dictionary<int, Country> Countries { get; init; }
    public required Dictionary<int, Commandery> Commanderies { get; init; }

    public GameEvent? CurrentGameEvent { get; set; } = null;

    private const int MaxRecentEvents = 3;
    public List<RecentEvent> RecentGameEvents { get; init; } = [];

    public GamePhase CurrentPhase { get; set; } = GamePhase.Start;
    public required List<Unit> Units { get; set; }

    /// <summary>
    /// Gets the domain-layer player country instance, based on the player's country code.
    /// </summary>
    /// <returns>The domain-layer player country instance.</returns>
    /// <exception cref="InvalidOperationException">The country does not exist inside <c>Countries</c>. This is a game-breaking error.</exception>
    public Country GetPlayerCountry()
    {
        if (!Countries.TryGetValue(PlayerCountryId, out var country))
            throw new InvalidOperationException(
                $"Player country '{PlayerCountryId}' not found in game state.");
        return country;
    }

    /// <summary>
    /// Pushes a recent event onto the recent event queue, which is bounded.
    /// Elements out of bound are automatically evicted.
    /// </summary>
    /// <param name="e"></param>
    public void PushRecentEvent(RecentEvent e)
    {
        RecentGameEvents.Add(e);
        if (RecentGameEvents.Count > MaxRecentEvents)
            RecentGameEvents.RemoveAt(0);
    }
}