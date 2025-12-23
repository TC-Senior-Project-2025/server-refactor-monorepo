using Microsoft.AspNetCore.Mvc;
using Server.Modules.Auth.Dto;
using Server.Modules.Users.Dto;

namespace Server.Modules.Auth;

/// <summary>
/// Controller for authentication-related endpoints including registration, login, and logout.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="dto">User registration data.</param>
    /// <returns>The created user information.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Explicitly map RegisterDto to CreateUserDto if we reused it, or just pass manual args.
        // AuthService expects CreateUserDto for signature re-use or we should update AuthService to take RegisterDto.
        // Let's create a CreateUserDto here.
        var user = await authService.Register(new CreateUserDto
        {
            Username = dto.Username,
            Password = dto.Password
        });

        return CreatedAtAction(nameof(Register), new { id = user.Id }, new { user.Id, user.Username });
    }

    /// <summary>
    /// Authenticates a user and creates a session.
    /// </summary>
    /// <param name="dto">Login credentials.</param>
    /// <returns>The authentication token.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var token = await authService.Login(dto.Username, dto.Password);

            // Set HttpOnly Cookie
            Response.Cookies.Append("access_token", token.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Ensure we are using HTTPS or localhost with dev certs
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { Token = token });
        }
        catch (Exception)
        {
            return Unauthorized("Invalid credentials");
        }
    }

    /// <summary>
    /// Logs out the current user by revoking their token and clearing cookies.
    /// </summary>
    /// <returns>A success message.</returns>
    [HttpDelete("logout")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> Logout()
    {
        // 1. Revoke the token if identified
        var tokenIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TokenId");
        if (tokenIdClaim != null && Guid.TryParse(tokenIdClaim.Value, out var tokenGuid))
        {
            await authService.RevokeToken(tokenGuid);
        }

        // 2. Clear Cookie
        Response.Cookies.Delete("access_token");

        return Ok(new { message = "Logged out successfully" });
    }
}
