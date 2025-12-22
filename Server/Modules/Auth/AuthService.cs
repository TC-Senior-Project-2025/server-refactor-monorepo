using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Users;
using Server.Modules.Users.Dto;
using BCrypt.Net;
using Server.Modules.Auth.Entities;
using Server.Modules.Users.Entities;

namespace Server.Modules.Auth;

/// <summary>
/// Service for handling user authentication operations including registration, login, and token management.
/// </summary>
public class AuthService(AppDbContext dbContext, ILogger<AuthService> logger)
{
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="dto">User registration data.</param>
    /// <returns>The newly created user.</returns>
    /// <exception cref="Exception">Thrown when a user with the same username already exists.</exception>
    public async Task<UserEntity> Register(CreateUserDto dto)
    {
        // Check if user exists
        if (await dbContext.Users.AnyAsync(u => u.Username == dto.Username))
        {
            throw new Exception("User already exists");
        }

        var paramPassword = dto.Password;
        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(paramPassword);

        var user = new UserEntity
        {
            Username = dto.Username,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Authenticates a user and creates a session token.
    /// </summary>
    /// <param name="username">The username to authenticate.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>A GUID token for the authenticated session.</returns>
    /// <exception cref="Exception">Thrown when credentials are invalid.</exception>
    public async Task<Guid> Login(string username, string password)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
        {
            logger.LogWarning("Invalid login attempt for username {Username}", username);
            throw new Exception("Invalid credentials");
        }

        // Create Session Token
        var token = Guid.NewGuid();
        var userToken = new UserTokenEntity
        {
            Token = token,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        dbContext.UserTokens.Add(userToken);
        await dbContext.SaveChangesAsync();

        return token;
    }

    /// <summary>
    /// Validates an authentication token and retrieves the associated user.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns>The user associated with the token, or null if the token is invalid or expired.</returns>
    public async Task<UserEntity?> ValidateToken(Guid token)
    {
        var userToken = await dbContext.UserTokens
            .Include(t => t.UserEntity)
            .FirstOrDefaultAsync(t => t.Token == token && t.ExpiresAt > DateTime.UtcNow);

        return userToken?.UserEntity;
    }

    /// <summary>
    /// Revokes an authentication token, effectively logging out the user.
    /// </summary>
    /// <param name="token">The token to revoke.</param>
    public async Task RevokeToken(Guid token)
    {
        var userToken = await dbContext.UserTokens.FindAsync(token);
        if (userToken != null)
        {
            dbContext.UserTokens.Remove(userToken);
            await dbContext.SaveChangesAsync();
        }
    }
}
