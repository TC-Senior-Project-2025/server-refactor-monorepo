using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Modules.Users.Dto;

namespace Server.Modules.Users;

/// <summary>
/// Controller for user management endpoints.
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController(UsersService usersService) : ControllerBase
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="dto">User creation data.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var user = await usersService.Create(dto);
        return CreatedAtAction(nameof(FindOne), new { id = user.Id }, user);
    }

    /// <summary>
    /// Retrieves all users. Requires authentication.
    /// </summary>
    /// <returns>A list of all users.</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        var users = await usersService.FindAll();
        return Ok(users);
    }

    /// <summary>
    /// Retrieves a specific user by ID. Requires authentication.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>The user data, or 404 if not found.</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> FindOne(int id)
    {
        var user = await usersService.FindOne(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Updates an existing user. Requires authentication.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="dto">The updated user data.</param>
    /// <returns>The updated user, or 404 if not found.</returns>
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var user = await usersService.Update(id, dto);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Deletes a user. Requires authentication.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <returns>204 No Content if successful, or 404 if not found.</returns>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await usersService.Delete(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
