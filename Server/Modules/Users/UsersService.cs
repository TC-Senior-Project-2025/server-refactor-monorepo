using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Users.Dto;
using Server.Modules.Users.Entities;

namespace Server.Modules.Users;

/// <summary>
/// Service for managing user-related operations.
/// </summary>
public class UsersService(AppDbContext dbContext)
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="dto">User creation data.</param>
    /// <returns>The created user data.</returns>
    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var user = new UserEntity
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return MapToDto(user);
    }

    /// <summary>
    /// Retrieves all users from the system.
    /// </summary>
    /// <returns>A list of all users.</returns>
    public async Task<List<UserDto>> FindAll()
    {
        var users = await dbContext.Users.ToListAsync();
        return users.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Retrieves a specific user by their ID.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>The user data, or null if not found.</returns>
    public async Task<UserDto?> FindOne(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        return user == null ? null : MapToDto(user);
    }

    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="dto">The updated user data.</param>
    /// <returns>The updated user data, or null if the user was not found.</returns>
    public async Task<UserDto?> Update(int id, UpdateUserDto dto)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null) return null;

        if (dto.Username != null) user.Username = dto.Username;

        await dbContext.SaveChangesAsync();
        return MapToDto(user);
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>True if the user was deleted, false if not found.</returns>
    public async Task<bool> Delete(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null) return false;

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return true;
    }

    private static UserDto MapToDto(UserEntity userEntity)
    {
        return new UserDto
        {
            Id = userEntity.Id,
            Username = userEntity.Username,
            CreatedAt = userEntity.CreatedAt
        };
    }
}
