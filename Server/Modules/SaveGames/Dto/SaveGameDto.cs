namespace Server.Modules.SaveGames.Dto;

/// <summary>
/// DTO representing a save game.
/// </summary>
public class SaveGameDto
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the user who owns the save.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The game state in JSON format.
    /// </summary>
    public string GameStateJson { get; set; } = string.Empty;
}
