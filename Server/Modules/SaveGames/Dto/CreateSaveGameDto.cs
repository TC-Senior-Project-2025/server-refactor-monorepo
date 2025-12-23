using System.ComponentModel.DataAnnotations;

namespace Server.Modules.SaveGames.Dto;

/// <summary>
/// DTO for creating a new save game.
/// </summary>
public class CreateSaveGameDto
{
    /// <summary>
    /// The name of the save game.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public required string SaveName { get; init; }
}
