using System.ComponentModel.DataAnnotations;

namespace Server.Modules.SaveGames.Dto;

/// <summary>
/// DTO for creating a new save game.
/// </summary>
public class CreateSaveGameDto
{
    /// <summary>
    /// The game state in JSON format.
    /// </summary>
    [Required]
    public string GameStateJson { get; set; } = string.Empty;
}
