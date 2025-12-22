using System.ComponentModel.DataAnnotations;

namespace Server.Modules.Auth.Dto;

/// <summary>
/// Data transfer object for user login requests.
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Gets or sets the username for authentication.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for authentication.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object for user registration requests.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the desired username for the new account.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the new account.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}
