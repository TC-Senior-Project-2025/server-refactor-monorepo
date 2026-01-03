using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Assets;
using Server.Modules.Game.Core;
using Server.Modules.SaveGames.Dto;
using Server.Modules.SaveGames.Entities;

namespace Server.Modules.SaveGames;

/// <summary>
/// Service for managing user save games.
/// </summary>
public class SaveGamesService(
    AppDbContext dbContext,
    AssetsService assetsService)
{
    /// <summary>
    /// Creates a new save game for a user.
    /// </summary>
    public async Task<SaveGameEntity> Create(int userId, CreateSaveGameDto dto)
    {
        // TODO: Variable scenario name
        var scenario = assetsService.LoadScenario("default_scenario");
        if (scenario == null)
        {
            throw new InvalidOperationException("Default scenario not found");
        }

        var countries = scenario.Country
            .ToDictionary(ce => ce.Id, ce => new Country()
            {
                Id = ce.Id,
                Code = ce.Code,
                Name = ce.Name,
                Resources = new NationalResources
                {
                    Efficiency = ce.Efficiency,
                    Treasury = ce.Treasury,
                    Manpower = ce.Manpower,
                    Stability = ce.Stability,
                    Prestige = ce.Prestige
                },
                RecentSituationSummary = ce.History
            });

        var commanderies = scenario.Commandery
            .ToDictionary(ce => ce.Id, ce => new Commandery()
            {
                Id = ce.Id,
                Code = ce.Code,
                Name = ce.Name,
                Population = ce.Population,
                Wealth = ce.Wealth,
                Unrest = ce.Unrest,
                CountryId = ce.CountryId
            });

        var units = scenario.Army.Select(u => new Unit
        {
            Id = u.Id,
            CountryId = u.CountryId,
            CommanderId = u.CommanderId,
            LocationId = u.LocationId,
            Name = u.Name,
            Size = u.Size,
            Morale = u.Morale,
            Supply = u.Supply,
            History = u.History
        }).ToList();

        var people = scenario.Person.Select(u => new Person
        {
            Id = u.Id,
            CountryId = u.CountryId,
            Name = u.Name,
            Age = u.Age,
            IsAlive = u.IsAlive,
            Loyalty = u.Loyalty,
            Role = u.Role,
            History = u.History
        }).ToList();

        var gameState = new GameState
        {
            Turn = 0,
            PlayerCountryId = 1,
            Countries = countries,
            Commanderies = commanderies,
            Units = units,
            People = people
        };

        var saveGame = new SaveGameEntity
        {
            SaveName = dto.SaveName,
            UserId = userId,
            GameStateJson = JsonSerializer.Serialize(gameState),
            CreatedAt = DateTime.UtcNow
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

    public async Task DeleteSaveGame(int id)
    {
        var saveGame = await GetSaveGame(id);
        if (saveGame == null) return;

        dbContext.SaveGames.Remove(saveGame);
        await dbContext.SaveChangesAsync();
    }
}
