using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Modules.Users;
using Server.Modules.Users.Entities;

namespace Server.Modules.SaveGames.Entities;

/// <summary>
/// Represents a saved game state for a user.
/// </summary>
[Table("save_games")]
public class SaveGameEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the save game.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the serialized game state.
    /// </summary>
    [Column("game_state_json")]
    public required string GameStateJson { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user who owns this save.
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user who owns this save.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity UserEntity { get; set; } = null!;
}