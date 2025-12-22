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

    public string? RecentSituationSummary { get; set; } = null;
    
    public required Dictionary<string, Country> Countries { get; init; }
    
    public GameEvent? CurrentGameEvent { get; set; } = null;

    public Country? GetPlayerCountry()
    {
        Countries.TryGetValue(PlayerCountryCode, out var country);
        return country;
    }
}