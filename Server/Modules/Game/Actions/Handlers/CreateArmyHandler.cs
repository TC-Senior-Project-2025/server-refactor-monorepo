using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.CreateUnit, typeof(CreateUnitDto))]
public class CreateUnitHandler(ILogger<CreateUnitHandler> logger) : IPlayerActionHandler
{
    private readonly ILogger<CreateUnitHandler> _logger = logger;

    public PlayerActionType Type => PlayerActionType.CreateUnit;
    public Type? PayloadType => typeof(CreateUnitDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        var dto = payload as CreateUnitDto ?? throw new InvalidOperationException("CreateUnit payload is missing");

        var locationId = dto.LocationId;

        // Validation: Check if location exists
        if (!gameState.Commanderies.TryGetValue(locationId, out var commandery))
        {
            throw new InvalidOperationException($"Commandery {locationId} does not exist");
        }

        // Validation: Check if player owns the location
        if (commandery.CountryId != gameState.PlayerCountryId)
        {
            throw new InvalidOperationException($"You do not own commandery {locationId}");
        }

        var playerCountry = gameState.GetPlayerCountry();
        int manpowerCost = dto.Manpower;

        // Validation: Check if player has enough manpower
        if (playerCountry.Resources.Manpower < manpowerCost)
        {
            throw new InvalidOperationException($"Not enough manpower! Required: {manpowerCost}, Available: {playerCountry.Resources.Manpower}");
        }

        if (manpowerCost <= 0)
        {
            throw new InvalidOperationException("Army size must be positive");
        }

        // Deduct resources
        playerCountry.Resources.SetManpower(m => m - manpowerCost);

        // Generate new ID
        int newId = (gameState.Units.Count > 0 ? gameState.Units.Max(u => u.Id) : 0) + 1;

        var newUnit = new Unit
        {
            Id = newId,
            CountryId = gameState.PlayerCountryId,
            CommanderId = null,
            LocationId = locationId,
            Name = dto.Name,
            Size = manpowerCost,
            Morale = 0,
            Supply = 100,
            History = $"Created on turn {gameState.Turn} at {commandery.Name}.",
            CanMove = true,
            CanResupply = true
        };

        gameState.Units.Add(newUnit);

        _logger.LogInformation("Unit '{Name}' created at {Location} with size {Size}. Manpower Cost: {Cost}.",
            newUnit.Name, commandery.Name, newUnit.Size, manpowerCost);

        return Task.CompletedTask;
    }
}
