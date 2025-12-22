using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Modules.Users.Entities;

namespace Server.Modules.Auth.Entities;

/// <summary>
/// Represents a user authentication token stored in the database.
/// </summary>
[Table("user_tokens")]
public class UserTokenEntity
{
    /// <summary>
    /// Gets or sets the unique token identifier.
    /// </summary>
    [Key]
    [Column("token")]
    public Guid Token { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user this token belongs to.
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with this token.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity UserEntity { get; set; } = null!;

    /// <summary>
    /// Gets or sets the expiration date and time of this token.
    /// </summary>
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this token was created.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
