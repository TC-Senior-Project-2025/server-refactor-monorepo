using Server.Modules.Game.Actions.Dto;
using Server.Modules.Game.Actions.Enums;
using Server.Modules.Game.Core;
using Server.Modules.Game.Services;

namespace Server.Modules.Game.Actions.Handlers;

[PlayerActionHandler(PlayerActionType.ResupplyUnit, typeof(ResupplyUnitDto))]
public class ResupplyUnitHandler(ILogger<ResupplyUnitHandler> logger) : IPlayerActionHandler
{
    private readonly ILogger<ResupplyUnitHandler> _logger = logger;

    public PlayerActionType Type => PlayerActionType.ResupplyUnit;
    public Type? PayloadType => typeof(ResupplyUnitDto);

    public Task HandleAsync(GameActionContext context, GameState gameState, object? payload)
    {
        var dto = payload as ResupplyUnitDto ?? throw new InvalidOperationException("ResupplyUnit payload is missing");

        var unit = gameState.Units.Find(u => u.Id == dto.UnitId)
            ?? throw new InvalidOperationException($"Unit {dto.UnitId} does not exist");

        if (!unit.CanResupply)
        {
            throw new InvalidOperationException("Unit cannot be resupplied");
        }

        if (unit.CountryId != gameState.PlayerCountryId)
        {
            throw new InvalidOperationException("You do not own this unit");
        }

        if (unit.Supply >= 100)
        {
            throw new InvalidOperationException("Army is already fully supplied");
        }

        double costPerSupply = ArmyService.CalculateCostPerSupply(unit.Size);
        int maxNeeded = 100 - unit.Supply;
        int amount = dto.Amount;

        if (amount <= 0)
        {
            throw new InvalidOperationException("Invalid amount");
        }
        if (amount > maxNeeded)
        {
            throw new InvalidOperationException($"You only need {maxNeeded} supply");
        }

        int totalCost = (int)Math.Ceiling(costPerSupply * amount);
        var playerCountry = gameState.GetPlayerCountry();

        if (totalCost > playerCountry.Resources.Treasury)
        {
            throw new InvalidOperationException($"Not enough gold! Cost: {totalCost}, Treasury: {playerCountry.Resources.Treasury}");
        }

        playerCountry.Resources.SetTreasury(t => t - totalCost);
        unit.Supply += amount;
        unit.Morale += amount / 5;
        unit.CanResupply = false;

        _logger.LogInformation("Unit {UnitId} supplied by {Amount}. Cost: {TotalCost}. New Supply: {Supply}. New Morale: {Morale}",
            unit.Id, amount, totalCost, unit.Supply, unit.Morale);

        return Task.CompletedTask;
    }
}
