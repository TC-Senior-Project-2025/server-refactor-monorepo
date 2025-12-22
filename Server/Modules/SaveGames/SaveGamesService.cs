using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Countries;
using Server.Modules.Game.Core;
using Server.Modules.SaveGames.Dto;
using Server.Modules.SaveGames.Entities;

namespace Server.Modules.SaveGames;

/// <summary>
/// Service for managing user save games.
/// </summary>
public class SaveGamesService(AppDbContext dbContext, CountriesService countriesService)
{
    /// <summary>
    /// Creates a new save game for a user.
    /// </summary>
    public async Task<SaveGameEntity> Create(int userId)
    {
        var countries = (await countriesService.GetCountries())
            .ToDictionary(ce => ce.Code, ce => new Country
            {
                Code = ce.Code,
                Name = ce.Name,
                Efficiency = ce.Efficiency,
                Treasury = ce.Treasury,
                Stability = ce.Stability,
                Manpower = ce.Manpower,
                Prestige = ce.Prestige
            });
        
        var gameState = new GameState
        {
            Turn = 1,
            PlayerCountryCode = "QIN",
            Countries = countries,
        };

        var saveGame = new SaveGameEntity
        {
            UserId = userId,
            GameStateJson = JsonSerializer.Serialize(gameState)
        };

        dbContext.SaveGames.Add(saveGame);
        await dbContext.SaveChangesAsync();

        return saveGame;
    }

    /// <summary>
    /// Lists all save games for a specific user.
    /// </summary>
    public async Task<List<SaveGameEntity>> ListForUser(int userId)
    {
        return await dbContext.SaveGames
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Id)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a specific save game by ID.
    /// </summary>
    /// <param name="id">The save game ID.</param>
    /// <returns>The save game if found, otherwise null.</returns>
    public async Task<SaveGameEntity?> GetSaveGame(int id)
    {
        return await dbContext.SaveGames.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateSaveGame(int id, GameState gameState)
    {
        var saveGame = await dbContext.SaveGames.FindAsync(id);
        if (saveGame == null) return;
        
        saveGame.GameStateJson = JsonSerializer.Serialize(gameState);
        
        await dbContext.SaveChangesAsync();
    }
}
