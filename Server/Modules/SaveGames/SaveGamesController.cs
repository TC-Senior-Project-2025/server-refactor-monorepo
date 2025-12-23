using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Modules.SaveGames.Dto;

namespace Server.Modules.SaveGames;

/// <summary>
/// Controller for managing save games.
/// </summary>
[ApiController]
[Route("api/save-games")]
[Authorize]
public class SaveGamesController(SaveGamesService saveGamesService) : ControllerBase
{
    /// <summary>
    /// Creates a new save game.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SaveGameDto>> Create(CreateSaveGameDto dto)
    {
        // "Id" claim is populated by StatefulAuthHandler from the user's ID
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var saveGame = await saveGamesService.Create(userId, dto);

        return Ok(saveGame);
    }

    /// <summary>
    /// Lists the current user's save games.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<SaveGameDto>>> ListMySaveGames()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var saveGames = await saveGamesService.ListForUser(userId);

        return Ok(saveGames);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteSaveGame(int id)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }
        
        var saveGame = await saveGamesService.GetSaveGame(id);
        if (saveGame == null) return NotFound("Save game not found");
        
        if (saveGame.UserId != userId) return Unauthorized();
        
        await saveGamesService.DeleteSaveGame(id);
        
        return Ok();
    }
}
