using System.ComponentModel.DataAnnotations;

namespace Server.Modules.Users.Dto;

/// <summary>
/// Data transfer object for updating an existing user.
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// Gets or sets the new username for the user. Null if not being updated.
    /// </summary>
    [MaxLength(50)]
    public string? Username { get; set; }
}
